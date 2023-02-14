using System.Collections;
using UnityEngine;

namespace Runner
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerStatsComponent))]
    public abstract class BasePlayerController : MonoBehaviour
    {
        protected Rigidbody _rigidbody;
        protected PlayerStatsComponent _playerStats;

        protected virtual void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerStats = GetComponent<PlayerStatsComponent>();
            StartCoroutine(MoveForward());
        }

        protected void Jump()
        {
            _rigidbody.AddForce(transform.up * _playerStats.JumpForce, ForceMode.Impulse);
        }

        private IEnumerator MoveForward()
        {
            while(true)
            {
                transform.position += transform.forward * _playerStats.ForwardSpeed * Time.deltaTime;
                yield return null;
			}
		}
    }
}
