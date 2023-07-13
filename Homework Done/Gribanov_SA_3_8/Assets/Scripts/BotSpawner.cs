using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AI
{
    public class BotSpawner : MonoBehaviour, IPointerClickHandler
    {
        private CameraController _camera;
        private WindowValuesHandler _zigguratWindow;

        [SerializeField]
        private BotSideType _botSide;

        [Space, SerializeField]
        private Units.BotComponent _botPrefab;
        [SerializeField]
        private Vector3 _spawnPoint;
        [SerializeField]
        private Vector3 _spawnTarget;
        [SerializeField]
        private GameObject _pool;
        [SerializeField]
        private Managers.AIManager _aiManager;

        [Space, SerializeField]
        private StatsData _botStats;

        public StatsData GetStats => _botStats;
        public int GetSpawnCount => _botSpawnCount;
        public float GetSpawnDelay => _botSpawnDelay;

        [Space, SerializeField]
        private int _botSpawnCount = 1;
        [SerializeField, Min(0)]
        private float _botSpawnDelay = 1;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.TransformPoint(_spawnPoint), 2);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(_spawnTarget), 2);
        }

        void Start()
        {
            _camera = Camera.main.GetComponent<CameraController>();
            if (_camera == null) Debug.LogError($"CameraController is not found");
            else
            {
                _zigguratWindow = _camera.GetComponentInChildren<WindowValuesHandler>();
                if(_zigguratWindow == null) Debug.LogError($"WindowValuesHandler is not found");
            }
            StartCoroutine(BotSpawn());
        }

        void Update()
        {

        }

        public void SetZigguratParams(StatsData stats, int botSpawnCount, float botSpawnDelay)
        {
            _botStats = stats;
            _botSpawnCount = botSpawnCount;
            _botSpawnDelay = botSpawnDelay;
        }

        private IEnumerator BotSpawn()
        {
            yield return new WaitForSeconds(_botSpawnDelay);
            while (true)
            {
                if (_botSpawnCount > 0)
                {
                    Dictionary<UnitType, StatsData> dictionary = new Dictionary<UnitType, StatsData>();
                    dictionary[_botPrefab.GetUnitType] = _botStats;
                    var bot = _aiManager.CreateNewEnemy(_botPrefab, dictionary, transform.TransformPoint(_spawnPoint), transform.eulerAngles);
                    bot.SideType = _botSide;
                    yield return new WaitForSeconds(1f);
                    //bot.Activity = true;
                    bot.SpawnTarget = new TargetPoint(transform.TransformPoint(_spawnTarget));
                    bot.Target = new TargetPoint(transform.TransformPoint(_spawnTarget));
                    bot.SteeringType = SteeringBehavioursType.Seek;
                    //bot.CurrentActionType = ActionType.MOVE;
                    
                    _botSpawnCount--;
                }
                yield return new WaitForSeconds(_botSpawnDelay);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogWarning($"{name} was selected");
            _zigguratWindow.OnZigguratSelect(this);
        }
    }
}