using System.Collections;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerConditionComponent : ConditionComponent
    {
        private bool _immortal = false;
        private Vector3 _startPoint;
        private SpriteRenderer _render;

        [SerializeField]
        private float _immortalTime = 3;
        [SerializeField]
        private float _immortalSwitchVisual = 0.2f;

        private void Start()
        {
            _startPoint = transform.position;
            _render = GetComponent<SpriteRenderer>();
        }

        public override void SetDamage(int damage)
        {
            if (_immortal) return;

            _health -= damage;
            transform.position = _startPoint;
            StartCoroutine(OnImmortal());
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator OnImmortal()
        {
            _immortal = true;
            var time = _immortalTime;
            while (time > 0f)
            {
                _render.enabled = !_render.enabled;
                time -= Time.deltaTime + _immortalSwitchVisual;
                yield return new WaitForSeconds(_immortalSwitchVisual);
            }

            _render.enabled = true;
            _immortal = false;
        }
    }
}