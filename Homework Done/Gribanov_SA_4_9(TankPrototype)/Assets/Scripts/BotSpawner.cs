using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class BotSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<SpawnPoint> _spawnPoints;
        [SerializeField]
        private BotComponent _botPrefab;

        public static BotSpawner Self;

        [SerializeField, Min(0)]
        private float _spawnDelay = 2f;
        [SerializeField, Min(1)]
        private int _maxBotsOnScene = 4;

        private int _botCount = 0;

        public int BotCount
        {
            get => _botCount;
            set
            {
                _botCount = value;
            }
        }

        private void Awake()
        {
            Self = this;
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (_botCount < _maxBotsOnScene)
                {
                    var pos = _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position;
                    Instantiate(_botPrefab, pos, new Quaternion());
                    _botCount++;
                }
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}