using UnityEngine;

namespace Tanks
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField, Min(0.5f)]
        private float _speed = 1f;

        public void OnMove(DirectionType type)
        {
            transform.position += Extensions.ConvertTypeFromDirection(type) * (Time.deltaTime * _speed);
            transform.eulerAngles = Extensions.ConvertTypeFromRotation(type);
        }
    }
}