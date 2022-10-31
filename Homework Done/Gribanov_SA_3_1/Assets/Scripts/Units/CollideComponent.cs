using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Units
{
    public class CollideComponent : MonoBehaviour
    {
        [SerializeField, Tooltip("Переменная, указывающая на то, является ли объект мячиком.")]
        private bool IsBall = false;
        private void OnCollisionEnter(Collision collision)
        {
            //шарик должен отражаться
            if (IsBall)
            {
                var direction = Managers.GameManager.Self.World._direction;
                direction = Vector3.Reflect(direction, collision.GetContact(0).normal);
                Managers.GameManager.Self.World._direction = direction;
            }
            else
            {
                Managers.GameManager.Self.World.DestroyBlock(gameObject);
            }
        }

    }
}
