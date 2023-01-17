using System.Collections;
using UnityEngine;

namespace Tanks
{
    public class FireComponent : MonoBehaviour
    {
        private bool _canFire = true;

        [SerializeField, Min(0)]
        private float _deltaFire = 0.25f;
        [SerializeField]
        private Projectile _prefab;
        [SerializeField]
        private SideType _side;

        public SideType GetSide => _side;

        public void OnFire()
        {
            if (!_canFire) return;

            var bullet = Instantiate(_prefab, transform.position, transform.rotation);
            bullet.SetParams(Extensions.ConvertRotationFromType(transform.eulerAngles), _side);

            StartCoroutine(OnDelay());
        }

        private IEnumerator OnDelay()
        {
            _canFire = false;
            yield return new WaitForSeconds(_deltaFire);
            _canFire = true;
        }
    }
}