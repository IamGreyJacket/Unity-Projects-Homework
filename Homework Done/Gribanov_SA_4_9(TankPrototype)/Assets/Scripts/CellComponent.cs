using UnityEngine;

namespace Tanks
{
    public class CellComponent : MonoBehaviour
    {
        private SpriteRenderer _render;

        [SerializeField]
        private bool _destroyProjectile;
        [SerializeField]
        private bool _destroyCell;

        public bool DestroyProjectile => _destroyProjectile;
        public bool DestroyCell => _destroyCell;

        private void Start()
        {
            _render = GetComponent<SpriteRenderer>();
        }

        private void OnDestroy()
        {
            if (_render != null) _render.enabled = false;
        }
    }
}