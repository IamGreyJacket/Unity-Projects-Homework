using UnityEngine;

namespace Runner
{
    public class PlayerStatsComponent : MonoBehaviour
    {
        [Range(1f, 100f)]
        public float JumpForce = 5f;
        [Range(1f, 100f)]
        public float ForwardSpeed = 5f;
        [Range(1f, 100f)]
        public float SideSpeed = 5f;
    }
}
