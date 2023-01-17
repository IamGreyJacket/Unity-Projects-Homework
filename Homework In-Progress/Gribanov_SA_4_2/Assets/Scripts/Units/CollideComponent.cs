
using UnityEngine;

namespace Arkanoid.Units
{
    public class CollideComponent : MonoBehaviour
    {
        [SerializeField, Tooltip("Переменная, указывающая на то, является ли объект мячиком.")]
        private bool _isBall = false;

        private void OnValidate()
        {
            if (_isBall)
            {
                if(gameObject.name != "PlayBall" || !gameObject.TryGetComponent<SphereCollider>(out SphereCollider component))
                {
                    _isBall = false;
                    Debug.LogWarning("This GameObject either is not a PlayBall, or doesn't have SphereCollider");
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //шарик должен отражаться
            if (_isBall)
            {
                var direction = Managers.GameManager.Self.World.Direction;
                direction = Vector3.Reflect(direction, collision.GetContact(0).normal);
                Managers.GameManager.Self.World.Direction = direction;
            }
            else
            {
                Managers.GameManager.Self.World.DestroyBlock(gameObject);
            }
        }

    }
}
