using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Balance
{
    [CreateAssetMenu(fileName = "newBalanceSO", menuName = "Configurations/Balance", order = 1)]
    public class BalanceSO : ScriptableObject
    {
        public float Damage;
        public float Health;
        public string Name;
        public bool IsPlayer;
    }
}