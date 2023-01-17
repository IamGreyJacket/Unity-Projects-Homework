using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Net
{
    public class PlayerController : MonoBehaviour, IPunObservable
    {
        public event Managers.EndGameNotifier OnPlayerLoseEvent;

        private Controls _controls;
        private Transform _bulletPool;
        private GameObject _camera;

        private Transform _target;
        [SerializeField]
        private ProjectileController _bulletPrefab;
        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private Slider _healthBar;
        [SerializeField]
        private PhotonView _photonView;

        [Space, SerializeField, Range(1f, 10f)]
        private float _moveSpeed = 2f;
        [SerializeField, Range(0.5f, 5f)]
        private float _maxSpeed = 2f;
        [SerializeField, Range(1f, 50f)]
        private float _health = 5f;

        [Space, SerializeField, Range(0.1f, 1f)]
        private float _attackDelay = 0.4f;
        [SerializeField, Range(0.1f, 1f)]
        private float _rotateDelay = 0.25f;
        [SerializeField]
        private Vector3 _firePoint;

        public bool IsAlive { get; private set; }

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
            }
        }

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _controls = new Controls();
            _bulletPool = FindObjectOfType<UnityEngine.EventSystems.EventSystem>().transform;
            _healthBar = GetComponentInChildren<Slider>();
            _healthBar.maxValue = Health;
            IsAlive = true;

            Managers.GameManager.Self.OnGameOverEvent += OnGameOver;
            FindObjectOfType<Managers.GameManager>().AddPlayer(this);
        }

        public void SetTarget(Transform target)
        {
            _target = target;

            if (!_photonView.IsMine) return;

            _controls.Player1.Enable();

            _camera = FindObjectOfType<Camera>().gameObject;

            StartCoroutine(MoveCamera());
            StartCoroutine(Fire());
            StartCoroutine(Focus());
            
        }

        private IEnumerator MoveCamera()
        {
            while (true)
            {
                _camera.transform.position = new Vector3(transform.position.x, _camera.transform.position.y, transform.position.z - 4);
                //var endPoint = new Vector3(transform.position.x, _camera.transform.position.y, transform.position.z - 4);
                //_camera.transform.position = Vector3.Lerp(_camera.transform.position, endPoint, .5f);
                yield return null;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(PlayerData.Create(this));
            }
            else
            {
                ((PlayerData)stream.ReceiveNext()).Set(this);
            }
        }
        

        private IEnumerator Fire()
        {
            while (true)
            {
                if (_target != null)
                {
                    var bullet = Instantiate(_bulletPrefab, _bulletPool);
                    bullet.transform.position = transform.TransformPoint(_firePoint);
                    bullet.transform.rotation = transform.rotation;
                    bullet.Parent = this.gameObject.name;
                    var photonBullet = PhotonNetwork.Instantiate(_bulletPrefab.name, bullet.transform.position, bullet.transform.rotation);
                    photonBullet.GetComponent<ProjectileController>().Parent = this.gameObject.name;
                }
                yield return new WaitForSeconds(_attackDelay);
            }
        }

        private IEnumerator Focus()
        {
            while (true)
            {
                transform.LookAt(_target);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                yield return new WaitForSeconds(_rotateDelay);
            }
        }

        void FixedUpdate()
        {
            if (!_photonView.IsMine) return;

            var direction = _controls.Player1.Movement.ReadValue<Vector2>();


            if (direction.x == 0 && direction.y == 0) return;

            var velocity = _rigidbody.velocity;
            velocity += new Vector3(direction.x, 0f, direction.y) * _moveSpeed * Time.fixedDeltaTime;

            velocity.y = 0f;
            velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
            _rigidbody.velocity = velocity;
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(_firePoint), 0.2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<ProjectileController>();
            if (!_photonView.IsMine || bullet == null || bullet.Parent == this.gameObject.name) return;
            
            Health -= bullet.GetDamage;
            _healthBar.value = Health;
            Destroy(other.gameObject);

            if (Health <= 0f && IsAlive)
            {
                Debugger.Log($"Player \"{PhotonNetwork.NickName}\" is dead");
                IsAlive = false;
                _target = null;
                OnPlayerLoseEvent?.Invoke(this);
            }
        }

        private void OnGameOver()
        {
            _target = null;
            StopAllCoroutines();
            _controls.Player1.Disable();
        }

    }
}