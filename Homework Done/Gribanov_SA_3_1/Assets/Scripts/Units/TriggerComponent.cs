using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Units
{
    public class TriggerComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("The ball is out of gamezone");
            Managers.GameManager.Self.ReduceHealth();
            Managers.GameManager.Self.ResetBall();
        }
    }
}
