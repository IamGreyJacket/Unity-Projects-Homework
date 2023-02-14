using UnityEngine;

namespace Runner
{
    public class OldPlayerController : BasePlayerController
    {
        protected override void Start()
        {
            base.Start();
        }

        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Jump();

            var direction = Input.GetAxis("Horizontal") * _playerStats.SideSpeed * Time.fixedDeltaTime;

            if (direction == 0f) return;
            _rigidbody.velocity += direction * transform.right;
        }
	}
}
