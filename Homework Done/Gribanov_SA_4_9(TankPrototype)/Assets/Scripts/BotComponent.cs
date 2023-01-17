using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    /*Танк должен выбирать направление и двигаться по нему.
     * В определенные моменты должен расчитывать выбор следующего направления и снова следовать ему.
     * Через некоторое время бот должен менять направление движения (Настраиваемо).
     * Бот должен всегда вести огонь.
     * При получении урона от игрока должен выбрать новое направление движения, предпочтительно в сторону игрока
     * Количество ботов на сцене должно настраиваться в редакторе (Либо GameManager, либо Extensions, раз он уже используется)
     *                                                   (Ну или описать BotSpawner как скрипт и в него запихнуть ограничение)
     */
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent))]
    public class BotComponent : MonoBehaviour
    {
        private DirectionType _lastType;
        private DirectionType _currentType = DirectionType.Up;

        private MoveComponent _moveComp;
        private FireComponent _fireComp;

        [SerializeField, Min(1)]
        private float _moveTime = 5;
        private float _currentMoveTime;
        private float _currentCollisionTime = 0f;
        //Плохой Tooltip, надо переформулировать
        [SerializeField, Min(0), Tooltip("Максимальное количество времени, которое бот может \"упираться\"в стену/чужой коллайдер")]
        private float _maxCollisionTime = 2f;

        private void Start()
        {
            _moveComp = GetComponent<MoveComponent>();
            _fireComp = GetComponent<FireComponent>();
            _currentType = (DirectionType)Random.Range(1, 5);
            StartCoroutine(Fire());
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            while (true)
            {
                _currentMoveTime = _moveTime;
                while (_currentMoveTime > 0)
                {
                    _moveComp.OnMove(_currentType);
                    _currentMoveTime -= Time.deltaTime;
                    yield return null;
                }
                ChangeDirection();
            }

        }
        private IEnumerator Fire()
        {
            while (true)
            {
                _fireComp.OnFire();
                yield return new WaitForSeconds(.2f);
            }
        }

        private void ChangeDirection()
        {
            _lastType = _currentType;
            _currentType = (DirectionType)Random.Range(1, 5);
            while (_currentType == _lastType)
            {
                _currentType = (DirectionType)Random.Range(1, 5);
            }
            _currentMoveTime = _moveTime;
        }

        public void ChangeDirection(DirectionType direction)
        {
            _lastType = _currentType;
            _currentType = direction;
            _currentMoveTime = _moveTime;
            Debug.Log($"Projectile forced bot to move {_currentType}");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<CellComponent>(out var cell))
            {
                if(cell.DestroyCell == false)
                {
                    ChangeDirection();
                    return;
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            _currentCollisionTime += Time.deltaTime;
            if(_currentCollisionTime >= _maxCollisionTime)
            {
                ChangeDirection();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _currentCollisionTime = 0f;
        }

        private void OnDisable()
        {
            BotSpawner.Self.BotCount--;
        }
    }
}