using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AI.Assistants;
using Zenject;
using AI.Units;
using AI.Configurations;
using System.Collections;

namespace AI.Managers
{
    public class AIManager : MonoBehaviour
    {
        //Ассистент, обновляющий поведение бота
        private StateAIAssistant _stateAssist;
        //Ассистент, проверяющий активность ботов
        private ActivityAssistant _activityAssistant;
        //Ассистент, рассчитывающий передвижение ботов
        private MoveAssistant _moveAssistant;

        //Пул для ботов
        private Transform _enemiesPool;

        [Inject]
        private LinkedList<BotComponent> _enemies;
        [Inject]
        private Dictionary<UnitType, StatsData> _params;
        [Inject]
        private SignalBus _actionSignal;

        [SerializeField, Tooltip("Конфигурация настроек менеджера и его ассистентов")]
        private AIManagerConfiguration _configs;
        [SerializeField]
        private AIConfiguration[] _botConfigs;
        [SerializeField]

		private void Start()
		{
            foreach (var enemy in _enemiesPool.GetComponentsInChildren<BotComponent>(true))
            {
                _enemies.AddLast(enemy);
                _stateAssist.ClearStateAI(enemy);
                enemy.Construct(_actionSignal, _params);
            }
 
            _moveAssistant.SetParams(_stateAssist, _configs.GetDistanceActivation, _configs.GetRangeWander, _configs.GetWanderStatusDuration, _configs.GetMinVelocity, _configs.GetSqrArrivalDistance);
            _activityAssistant.Execute(_configs.GetActivityCheckDelay, _configs.GetDistanceActivation, _configs.GetTargetFocusDistance);
            _stateAssist.SetConfiguration(_configs.GetActionResultWeights);
            StartCoroutine(CheckHealthBot());
        }

		void Update()
        {
            _moveAssistant.OnMoves();
            OnStatusUpdate();

            #if UNITY_EDITOR
            UpdateEditorInfo();
            #endif
        }

        public void SetBotConfig(AIConfiguration botConfig)
        {
            _botConfigs = new AIConfiguration[] { botConfig };
        }

        public BotComponent CreateNewEnemy(BotComponent prefab, Dictionary<UnitType, StatsData> botStats,  Vector3 position, Vector3 rotation)
        {
            var bot = Instantiate(prefab, _enemiesPool);
            bot.transform.position = position;
            bot.transform.eulerAngles = rotation;
            _enemies.AddLast(bot);
            //Инъекция сущностей в объект
            bot.SetComponents();
            bot.Construct(_actionSignal, botStats);
            bot.Construct(_botConfigs);
            return bot;
        }

        private IEnumerator CheckHealthBot()//todo
        {
            while (true)
            {
                var removeUnits = new LinkedList<BotComponent>();
                foreach (var enemy in _enemies)
                {
                    if (enemy.GetProperties.Health <= 0f)
                    {
                        removeUnits.AddLast(enemy);
                        foreach(var target in _enemies)
                        {
                            if (removeUnits.Contains(target)) continue;
                            if (target.Target.GetPoint == enemy.transform.position) target.Target = new TargetPoint();
                            Debug.LogWarning(target.Target.GetPoint);
                        }
                    }
                }
                if (removeUnits.Count > 0) RemoveEnemies(removeUnits);
                yield return new WaitForSeconds(2f);
            }
        }

        public void RemoveEnemies(IEnumerable<BotComponent> targets)
        {
            foreach (var enemy in targets)
            {
                _enemies.Remove(enemy);
                Destroy(enemy.gameObject);
            }
		}

        //Обновление состояния статусов ботов
        private void OnStatusUpdate()
        {
            foreach (var bot in _enemies)
            {
                var removeStatuses = new LinkedList<StatusDataArgs>();

                foreach (var status in bot.GetStatuses)
                {
                    //Перманентные статусы не трогаются
                    if (status.Type == StatusType.Persistance) continue;

                    status.CurrentDuraction -= Time.deltaTime;
                    //Если : действие статуса закончено
                    if (status.CurrentDuraction > 0f) continue;

                    //Убираем закончившиеся статусы
                    switch (status.Type)
                    {
                        //Одиночный просто удаляется
                        case StatusType.Single:
                            removeStatuses.AddLast(status);
                            break;
                    }
                }
                //Сообщаем боту о том, какие статусы нужно удалить
                bot.RemoveStatuses(removeStatuses);
            }
        }

        [Inject]
        private void AssistantsConstruct(StateAIAssistant stateAIAssistant, ActivityAssistant activityAssistant, MoveAssistant moveAssistant)
        {
            _stateAssist = stateAIAssistant; _activityAssistant = activityAssistant; _moveAssistant = moveAssistant;
        }

        #region Editor
#if UNITY_EDITOR

        [Space, Header("---Debug data---"), ReadOnly, SerializeField]
        private int _allBotsCount;

        [ReadOnly, SerializeField]
        private int _activityBotsCount;

        private void UpdateEditorInfo()
        {
            _allBotsCount = _enemies.Count;
            _activityBotsCount = _enemies.Count(t => t.Activity);
        }

        private void OnValidate()
        {
            _enemiesPool = FindObjectsOfType<Transform>().First(t => t.name == "EnemiesPool");
        }
#endif
		#endregion
	}
}