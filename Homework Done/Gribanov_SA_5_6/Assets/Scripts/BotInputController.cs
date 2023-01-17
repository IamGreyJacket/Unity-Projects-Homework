using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    public class BotInputController : BaseInputController
    {
        private int _index;

        [SerializeField]
        private BotTargetPoint[] _points;

        protected override void FixedUpdate()
        {

        }

        private float GetAngle()
        {
            var targetPos = _points[_index].transform.position;
            targetPos.y = transform.position.y;

            var direction = targetPos - transform.position;

            return Vector3.SignedAngle(direction, transform.forward, transform.up);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<BotTargetPoint>() != null)
            {
                _index++;
            }
        }
    }
}