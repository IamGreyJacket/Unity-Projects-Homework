using UnityEngine;
using UnityEngine.InputSystem;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent))]
    public class InputComponent : MonoBehaviour
    {
        private DirectionType _lastType;

        private MoveComponent _moveComp;
        private FireComponent _fireComp;

        [SerializeField]
        private InputAction _move;
        [SerializeField]
        private InputAction _fire;

        private void Start()
        {
            _moveComp = GetComponent<MoveComponent>();
            _fireComp = GetComponent<FireComponent>();
            Debug.Log(new Vector2(-1.1f, 1.0f) - new Vector2(0f, 0f));
            _move.Enable();
            _fire.Enable();
        }

        private void Update()
        {
            
            var fire = _fire.ReadValue<float>();
            if (fire == 1f) _fireComp.OnFire();

            var direction = _move.ReadValue<Vector2>();
            DirectionType type;
            if (direction.x != 0f && direction.y != 0f)
            {
                type = _lastType;
            }
            else if (direction.x == 0f && direction.y == 0f) return;
            else
            {
                type = _lastType = Extensions.ConvertDirectionFromType(direction);
            }

            _moveComp.OnMove(type);
        }
    }
}