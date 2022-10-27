using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gribanov_SA_2_5
{
    
    public class EnemyController : MonoBehaviour
    {
        public EnemyData enemyData;
        private PlayerController _player = GameManager.Self._player;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Vector3.Distance(this.transform.position, _player.transform.position) <= enemyData.AttackRange)
            {
                GameManager.Self.Shoot(transform);
            }
        }
    }
}
