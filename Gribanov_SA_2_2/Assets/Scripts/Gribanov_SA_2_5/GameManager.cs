using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gribanov_SA_2_5
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static GameManager Self;
        private List<GameObject> _bullets;
        private List<ProjectileData> _bulletsData;
        private int bulletCount;
        private List<EnemyController> _enemies;
        private List<EnemyData> _enemiesData;
        private int enemyCount;
        [SerializeField]
        public PlayerController _player;
        [SerializeField]
        private GameObject _bullet;
        [SerializeField]
        private GameObject _enemy;
        void Awake()
        {
            Self = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            //Здесь отслеживаются статы врагов и пуль.
            for(int i = 0; i < _bullets.Count; i++)
            {
                if (_bulletsData[i].LivingTime <= 0)
                {
                    Destroy(_bullets[i]);
                    _bulletsData.Remove(_bulletsData[i]);
                }
                else
                {
                    _bulletsData[i].LivingTime -= Time.deltaTime;
                }
            }
            for (int i = 0; i < _enemies.Count; i++)
            {
                if(_enemiesData[i].Health.Health <= 0)
                {
                    Destroy(_enemies[i]);
                    _enemiesData.Remove(_enemiesData[i]);
                }
            }
        }

        public void MoveOpjects()
        {
            //Перемещение пуль
            for(int i = 0; i < bulletCount; i++)
            {
                _bullets[i].transform.position += transform.forward * Time.deltaTime * _bulletsData[i].Speed;
            }
        }

        public void Shoot(Transform pos)
        {
            //Создание пули.
            _bullets.Add(Instantiate(_bullet, pos.position, pos.rotation));
            ProjectileData temp = new ProjectileData();
            _bulletsData.Add(temp);
            bulletCount++;
        }

        public void SpawnEnemy()
        {
            var temp = Instantiate(_enemy, new Vector3(10, 0, 10), new Quaternion(0, 0, 0, 0));
            _enemies.Add(temp.GetComponent<EnemyController>());
            _enemiesData.Add(new EnemyData());
            _enemies[enemyCount + 1].enemyData = _enemiesData[enemyCount + 1];
            enemyCount++;
        }

    }
}
