using UnityEngine;

namespace Checkers
{
    public delegate void Notifier(ColorType winningColor);
    
    public class TriggerComponent : MonoBehaviour
    {
        /// <summary>
        /// Color, that trigger would recognize and activate "Game Over" script
        /// </summary>
        [SerializeField]
        private ColorType _condition;

        public ColorType GetColor => _condition;

        public Notifier OnTriggerEnterEvent;

        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ChipComponent>(out var chip))
            {
                if (chip.GetColor == _condition)
                {
                    Debug.LogWarning("And it's the winner!");
                    OnTriggerEnterEvent?.Invoke(chip.GetColor);
                    return;
                }
            }
        }
    }
}