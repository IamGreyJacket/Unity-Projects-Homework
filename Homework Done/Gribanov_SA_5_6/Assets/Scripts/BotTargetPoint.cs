using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    public class BotTargetPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 12f);
        }
    }
}