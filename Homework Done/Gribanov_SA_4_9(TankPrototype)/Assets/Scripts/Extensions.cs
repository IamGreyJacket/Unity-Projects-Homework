using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    public static class Extensions
    {
        private static Dictionary<DirectionType, Vector3> _directions;
        private static Dictionary<DirectionType, Vector3> _rotations;

        static Extensions()
        {
            _directions = new Dictionary<DirectionType, Vector3> 
            {
                { DirectionType.Up, new Vector3(0f, 1f, 0f)},
                { DirectionType.Down, new Vector3(0f, -1f, 0f)},
                { DirectionType.Left, new Vector3(-1f, 0f, 0f)},
                { DirectionType.Right, new Vector3(1f, 0f, 0f)}
            };

            _rotations = new Dictionary<DirectionType, Vector3> 
            {
                { DirectionType.Up, new Vector3(0f, 0f, 0f)},
                { DirectionType.Down, new Vector3(0f, 0f, 180f)},
                { DirectionType.Left, new Vector3(0f, 0f, 90f)},
                { DirectionType.Right, new Vector3(0f, 0f, 270f)}
            };
        }

        public static Vector3 ConvertTypeFromDirection(DirectionType type) 
            => _directions[type];
        public static DirectionType ConvertDirectionFromType(Vector3 direction) 
            => _directions.First(t => t.Value == direction).Key;
        public static DirectionType ConvertDirectionFromType(Vector2 direction)
        {
            var dir = (Vector3)direction;
            return _directions.First(t => t.Value == dir).Key;
        }
        public static Vector3 ConvertTypeFromRotation(DirectionType type)
             => _rotations[type];
        public static DirectionType ConvertRotationFromType(Vector3 rotation)
            => _rotations.First(t => t.Value == rotation).Key;
    }

    public enum DirectionType : byte
    {
        Error, Up, Right, Down, Left
    }

    public enum SideType : byte
    {
        None, Player, Enemy
    }
}