using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gribanov_SA_2_5
{
    public struct HealthBar
    {
        public int Health;
    }
    public class EnemyData
    {
        public enum EnemyType
        {
            one,
            two,
            three
        }
        public HealthBar Health;

        public float AttackSpeed = 1;
        public float AttackRange = 10;
        public float Speed = 2;
    }

    public class ProjectileData
    {
        public enum ProjectileType
        {
            first,
            second,
            third
        }
        public int Damage = 1;
        public float Speed = 4;
        public float LivingTime = 15;
    }
}