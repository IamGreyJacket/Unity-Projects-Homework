using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Units
{
    public class CollideComponent : MonoBehaviour
    {
        [SerializeField]
        private bool IsBall = false;
        private void OnCollisionEnter(Collision collision)
        {
            //шарик должен отражаться
            if (IsBall)
            {
                //Вот эту суку надо написать
            }
            else
            {
                Managers.GameManager.Self.world.DestroyBlock(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
