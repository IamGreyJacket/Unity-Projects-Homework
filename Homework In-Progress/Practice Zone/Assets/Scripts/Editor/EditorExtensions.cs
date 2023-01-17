using Newtonsoft.Json;

using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;

namespace Balance.Editor
{
    public class EditorExtensions
    {
        private static string _filePath = "Resources/Configurations";
        private static string _prefabPath = "Unit";
        private static string _SOPath = "newBalanceSO";

        [MenuItem("Configurations/Get Data")]//Новое подменю вверху редактора
        private static void GetData()
        {
            var unit = Resources.Load<UnitController>(_prefabPath);
            var path = string.Concat(Application.dataPath, "/", _filePath, "/", unit.name, ".json");
            var data = JsonConvert.SerializeObject(unit.Data);

            using (var stream = new StreamWriter(path))
            {
                using (var writer = new JsonTextWriter(stream))
                {
                    writer.WriteValue(data);
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("Get data");
        }

        [MenuItem("Configurations/Set Data")]//Новое подменю вверху редактора
        private static void SetData()
        {
            var unit = Resources.Load<UnitController>(_prefabPath);
            var path = string.Concat(Application.dataPath, "/", _filePath, "/", unit.name, ".json");
            var text = string.Empty;

            using (var stream = new StreamReader(path))
            {
                using (var reader = new JsonTextReader(stream))
                {
                    text = reader.ReadAsString();
                }
            }

            var data = JsonConvert.DeserializeObject<BaseParamsData>(text);
            unit.Data = data;

            AssetDatabase.Refresh();
            Debug.Log("Set data");
        }

        [MenuItem("Configurations/Get Balance")]//Новое подменю вверху редактора
        private static void GetBalance()
        {
            var SO = Resources.Load<BalanceSO>(_SOPath);
            var path = string.Concat(Application.dataPath, "/", _filePath, "/", "testText", ".json");
            var data = JsonConvert.SerializeObject(SO);

            using (var stream = new StreamWriter(path))
            {
                using (var writer = new JsonTextWriter(stream))
                {
                    writer.WriteValue(data);
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("Get balance");
        }

        [MenuItem("Configurations/Set Balance")]//Новое подменю вверху редактора
        private static void SetBalance()
        {
            var SO = ScriptableObject.CreateInstance<BalanceSO>();
            var path = string.Concat(Application.dataPath, "/", _filePath, "/", "testText", ".json");
            var text = string.Empty;

            using (var stream = new StreamReader(path))
            {
                using (var reader = new JsonTextReader(stream))
                {
                    text = reader.ReadAsString();
                }
            }

            var data = JsonConvert.DeserializeObject<BalanceSO>(text);

            SO.Damage = data.Damage;
            SO.Health = data.Health;
            SO.Name = data.Name;
            SO.IsPlayer = data.IsPlayer;

            AssetDatabase.CreateAsset(SO, "Assets/Resources/newBalanceSOv2.asset");
            AssetDatabase.Refresh();
            Debug.Log("Set balance");
        }
    }
}