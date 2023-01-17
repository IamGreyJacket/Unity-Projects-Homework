using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    public class StartFinishComponent : MonoBehaviour
    {
        [SerializeField]
        private bool _isStart;

        private void OnTriggerEnter(Collider other)
        {
            if (_isStart)
            {
                Debug.Log("Something hit a startline");
                GameManager.Self.StartStopwatch();
            }
            else
            {
                Debug.Log("Something hit a finishline");
                GameManager.Self.StopStopwatch();
                StartCoroutine(GameManager.Self.Finish());
            }
        }
    }
}