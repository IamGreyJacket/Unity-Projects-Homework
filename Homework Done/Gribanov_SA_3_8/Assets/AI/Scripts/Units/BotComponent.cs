using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using Zenject;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AI.Units
{
    public class BotComponent : BaseUnit, IPointerClickHandler
    {
        private SteeringData _steeringData;

        [SerializeField, Tooltip("Паттерн поведения бота"), ReadOnly(WriteInEditor : true)]
        private AIPattern _pattern;

        /// <summary>
        /// Текущее действие бота
        /// </summary>
        public ActionType CurrentActionType
        {
            get => _currentActionType;
            set
            {
                var oldState = _currentActionType;
                _currentActionType = value;

#if UNITY_EDITOR
                DebugManager.Log($"{name} start action : {value}", typeof(BaseUnit));
#endif

                if (value != ActionType.MOVE && value != ActionType.IDLE)
                {
                    SetAnimationState(value);
                    transform.LookAt(Target.GetPoint);//todo тоже резкий поворот надо исправить
                }
            }
        }

        /// <summary>
        /// Текущее состояние весов действий
        /// </summary>
        public WeightEnum<ActionType> CurrentActionWeightsAI { get; private set; }
        /// <summary>
        /// Изменения весов действий
        /// </summary>
        public WeightEnum<ActionType> DeltaActionWeightsAI { get; private set; }

        /// <summary>
        /// Активен-ли бот
        /// </summary>
        public bool Activity { get; set; }
        /// <summary>
        /// Преследует-ли бот кого-либо
        /// </summary>
        public bool Hunt { get; set; }
        /// <summary>
        /// Целевая точка в пространстве
        /// </summary>
        public TargetPoint Target {get; set; }

        public TargetPoint SpawnTarget { get; set; }

        public BotSideType SideType { get; set; }

        private Color _sideColor = Color.black;

        /// <summary>
        /// Тип перемещения
        /// </summary>
        public SteeringBehavioursType SteeringType { get; set; } = SteeringBehavioursType.Wander;

        /// <summary>
        /// Параметры перемещения
        /// </summary>
        public ref SteeringData GetSteeringData => ref _steeringData;


        /// <summary>
        /// Конфигурирование бота
        /// </summary>
        /// <param name="environments">Данные для окружения</param>
        [Inject]
        public void Construct(Configurations.AIConfiguration[] configs)
        {
            var config = configs.FirstOrDefault(t => t.GetPatternType == _pattern);//todo после старта не обработает без фабрики
            _steeringData = config.GetSteeringData;
#if UNITY_EDITOR

            if (config == null)
            {
                Debug.LogError("Бот не получил конфигурацию своего паттерна : " + _pattern);
                EditorApplication.isPaused = true;
			}
#endif
            CurrentActionWeightsAI = new WeightEnum<ActionType>(config.GetActionWeights);
            DeltaActionWeightsAI = new WeightEnum<ActionType>(config.GetActionWeights);
            
        }

        /// <summary>
        /// Возвращает интервал необходимого расстояния для указанного типа действия
        /// </summary>
        /// <param name="type">Тип действия</param>
        /// <returns>Требуемый интервал расстояния</returns>
        public Interval<float> GetExpectedInterval(ActionType type)
        {
            if (_actionData == null) _actionData = AIUtility.GetKeysForActionTypes(_unit);
            return _actionData[type].Interval;
        }


        /// <summary>
        /// Конструирование юнита
        /// </summary>
        /// <param name="signal">Сигнал</param>
        public void Construct(SignalBus signal, Dictionary<UnitType, StatsData> @params)//todo
        {
            _signal = signal;
            _propertiesAssistant = new UnitPropertiesAssistant(@params[_unit]);
            foreach (var element in _elements) element.Construct(this, signal);
        }

        protected override void Start()
        {
            base.Start();

            var mesh = GetComponentInChildren<SkinnedMeshRenderer>();
            _sideColor = mesh.material.color;

#if UNITY_EDITOR
            _visual = gameObject.AddComponent<AIStateVisualization>();
#endif
        }

        public Vector3 GetVelocity(IgnoreAxisType ignore = IgnoreAxisType.Y)
        {
            return IgnoreAxisUpdate(ignore, _rigidBody.velocity);
        }

        public void SetVelocity(Vector3 velocity, IgnoreAxisType ignore = IgnoreAxisType.None)
        {
            OnMove(velocity);

            //todo пока что персонаж резко поворачивается в сторону движения
            transform.LookAt(Target.GetPoint);

            _rigidBody.velocity = IgnoreAxisUpdate(ignore, velocity);
        }

        private Vector3 IgnoreAxisUpdate(IgnoreAxisType ignore, Vector3 velocity)
        {
            if (ignore == IgnoreAxisType.None) return velocity;
            if ((ignore & IgnoreAxisType.X) == IgnoreAxisType.X) velocity.x = 0f;
            if ((ignore & IgnoreAxisType.Y) == IgnoreAxisType.Y) velocity.y = 0f;
            if ((ignore & IgnoreAxisType.Z) == IgnoreAxisType.Z) velocity.z = 0f;

            return velocity;
        }

        private IEnumerator CheckHealth()
        {
            while (true)
            {
                if (GetProperties.Health <= 0) Destroy(this);
                yield return new WaitForSeconds(2f);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var mesh = GetComponentInChildren<SkinnedMeshRenderer>();
            if(mesh.material.color == Color.white)
            {
                mesh.material.color = _sideColor;
            }
            else
            {
                mesh.material.color = Color.white;
            }
        }

#if UNITY_EDITOR

        private AIStateVisualization _visual;

        private void Update()
        {
            _visual.Visualization(CurrentActionType, Activity, SteeringType, CurrentActionWeightsAI, DeltaActionWeightsAI, Target.GetPoint);
        }

#endif
    }
}
