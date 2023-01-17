using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent))]
    public class Projectile : MonoBehaviour
    {
        private SideType _side;
        private DirectionType _direction;

        private MoveComponent _moveComp;

        [SerializeField, Min(1)]
        private int _damage = 1;
        [SerializeField, Min(1)]
        private float _lifeTime = 3f;

        public SideType GetSide => _side;

        private void Start()
        {
            _moveComp = GetComponent<MoveComponent>();
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            _moveComp.OnMove(_direction);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        { 
            var fire = collision.GetComponent<FireComponent>();
            if(fire != null)
            {
                if (fire.GetSide == _side) return;

                var condition = fire.GetComponent<ConditionComponent>();
                condition.SetDamage(_damage);
                var bot = fire.GetComponent<BotComponent>();
                if(bot != null)
                {
                    var hitPoint = Vector2.ClampMagnitude((Vector2)transform.position - (Vector2)bot.transform.position, 1);
                    float maxValue = Mathf.Max(hitPoint.x, hitPoint.y);
                    float minValue = Mathf.Min(hitPoint.x, hitPoint.y);

                    if(minValue < 0)
                    {
                        if(minValue * -1 >= maxValue)
                        {
                            maxValue = minValue;
                        }
                    }
                    for(int i = 0; i < 2; i++)
                    {
                        if (hitPoint[i] == maxValue)
                        {
                            if (hitPoint[i] < 0) hitPoint[i] = -1;
                            else hitPoint[i] = 1;
                        }
                        else hitPoint[i] = 0;
                    }
                    bot.ChangeDirection(Extensions.ConvertDirectionFromType(hitPoint));
                }
                Destroy(gameObject);
                return;
            }

            var cell = collision.GetComponent<CellComponent>();
            if (cell != null)
            {
                if (cell.DestroyProjectile) Destroy(gameObject);
                if (cell.DestroyCell) Destroy(cell.gameObject); 
                return;
            }
        }

        public void SetParams(DirectionType direction, SideType side) 
            => (_direction, _side) = (direction, side);
    }
}