using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;
using UnityEditor.ShortcutManagement;

using UnityEngine;

using Object = UnityEngine.Object;

public class CustomInspectorWindow : EditorWindow
{
	private Dictionary<GameObject, MonoBehaviour[]> _currentScripts;
	private Vector2 _scroll;
	private MonoBehaviour _currentScript;

    [MenuItem("Extensions/Windows/Custom Inspector"), Shortcut("CustomInspector", KeyCode.C, ShortcutModifiers.Shift)]
    public static void OpenWindow()
    {
        var window = GetWindow<CustomInspectorWindow>(false, "References Inspector", true);
        window.minSize = new Vector2(500f, 500f);
    }

    private void OnEnable()
    {
		Selection.selectionChanged += Repaint;
    }

    private void OnSelectionChange()
    {
		var gameObjects = Selection.gameObjects;
		if (gameObjects == null)
		{
			_currentScripts = null;
			return;
		}

		_currentScripts = new Dictionary<GameObject, MonoBehaviour[]>();

		foreach(var GO in gameObjects)
        {
			_currentScripts[GO] = GO.GetComponents<MonoBehaviour>();
        }

	}

    private void PrintScripts()
    {
		if (_currentScripts == null) return;
		_scroll = EditorGUILayout.BeginScrollView(_scroll);
        EditorGUILayout.BeginVertical("box");

		var style = new GUIStyle("label")
		{
			fontSize = 15,
			fontStyle = FontStyle.Normal,
			alignment = TextAnchor.MiddleCenter,
			richText = true
		};

		foreach (var GO in _currentScripts.Keys)
		{
			EditorGUILayout.LabelField($"Game Object: <b>{GO.name}</b>", style);
			EditorGUILayout.Space(20f);
			foreach (var script in _currentScripts[GO])
			{
				var type = script.GetType();
				EditorGUILayout.LabelField($"Script: <i>{type}</i>", style);
				PrintFields(script); //Написать метод принтующий все поля сразу после имени
				EditorGUILayout.Space(10f);
			}
		}
        EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
	}

	private void PrintFields(MonoBehaviour script)
    {
		_currentScript = script;
		var fields = script.GetType().GetRuntimeFields();
        foreach (var field in fields)
        {
			if (field.IsPrivate && field.GetCustomAttributes(typeof(SerializeField), false).Length == 0) continue;
			PrintValue(field); //Принтует значение поля, соответственно его типу.
		}
    }

	private void PrintValue(FieldInfo field)
	{
		EditorGUILayout.BeginHorizontal("box");

		EditorGUILayout.LabelField($"{field.Name}");
		var type = field.FieldType;

		if (type.IsPrimitive && type.IsValueType) PrintPrimitive(field);
		else if (type.IsEnum) PrintEnum(field);
		else if (!type.IsPrimitive && type.IsValueType) PrintComplexValue(field);
		else if (type.IsClass) PrintObject(field);
		else if (type.IsArray || type.GetInterface(nameof(IEnumerable)) != null) PrintArray(field);

		EditorGUILayout.EndHorizontal();
	}

	private void PrintArray(FieldInfo field)
	{

		EditorGUILayout.BeginHorizontal("box");
		GUI.color = Color.red;
		EditorGUILayout.LabelField("Массивы не поддерживаются");
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();


		/*
		dynamic array = field.GetValue(_currentComponent);

		EditorGUILayout.LabelField(array.Length.ToString());

		var length = array.Length;
		length = EditorGUILayout.TextField(length);

		if(length > array.Length)
		{
			//ArrayUtility.
		}
		*/
	}

	private void PrintPrimitive(FieldInfo field)
	{
		var value = field.GetValue(_currentScript);
		if (field.FieldType == typeof(int))
		{
			var attr = field.GetCustomAttribute(typeof(RangeAttribute)) as RangeAttribute;

			if (attr == null)
			{
				field.SetValue(_currentScript, EditorGUILayout.IntField((int)value));
			}
			else
			{
				field.SetValue(_currentScript, EditorGUILayout.IntSlider((int)value, (int)attr.min, (int)attr.max));
			}
		}
		else if (field.FieldType == typeof(float))
		{
			var attr = field.GetCustomAttribute(typeof(RangeAttribute)) as RangeAttribute;

			if (attr == null)
			{
				field.SetValue(_currentScript, EditorGUILayout.FloatField((float)value));
			}
			else
			{
				field.SetValue(_currentScript, EditorGUILayout.Slider((float)value, attr.min, attr.max));
			}
		}
		else if (field.FieldType == typeof(bool))
		{
			field.SetValue(_currentScript, EditorGUILayout.Toggle((bool)value));
		}
		else if (field.FieldType == typeof(long))
		{
			field.SetValue(_currentScript, EditorGUILayout.LongField((long)value));
		}
		else if (field.FieldType == typeof(double))
		{
			field.SetValue(_currentScript, EditorGUILayout.DoubleField((double)value));
		}
	}

	private void PrintComplexValue(FieldInfo field)
	{
		var value = field.GetValue(_currentScript);
		if (field.FieldType == typeof(Vector2))
		{
			field.SetValue(_currentScript, EditorGUILayout.Vector2Field((string)null, (Vector2)value));
		}
		else if (field.FieldType == typeof(Vector3))
		{
			field.SetValue(_currentScript, EditorGUILayout.Vector3Field((string)null, (Vector3)value));
		}
		else if (field.FieldType == typeof(Vector2Int))
		{
			field.SetValue(_currentScript, EditorGUILayout.Vector2IntField((string)null, (Vector2Int)value));
		}
		else if (field.FieldType == typeof(Vector3Int))
		{
			field.SetValue(_currentScript, EditorGUILayout.Vector3IntField((string)null, (Vector3Int)value));
		}
		else if (field.FieldType == typeof(Vector4))
		{
			field.SetValue(_currentScript, EditorGUILayout.Vector4Field((string)null, (Vector4)value));
		}
		else if (field.FieldType == typeof(Quaternion))
		{
			var value1 = (Quaternion)field.GetValue(_currentScript);
			var vector = EditorGUILayout.Vector4Field((string)null, new Vector4(value1.x, value1.y, value1.z, value1.w));
			field.SetValue(_currentScript, new Quaternion(vector.x, vector.y, vector.z, vector.w));
		}
		else if (field.FieldType == typeof(Color))
		{
			field.SetValue(_currentScript, EditorGUILayout.ColorField((Color)value));
		}
		else if (field.FieldType == typeof(BoundsInt))
		{
			field.SetValue(_currentScript, EditorGUILayout.BoundsIntField((BoundsInt)value));
		}
		else if (field.FieldType == typeof(Bounds))
		{
			field.SetValue(_currentScript, EditorGUILayout.BoundsField((Bounds)value));
		}
		else if (field.FieldType == typeof(RectInt))
		{
			field.SetValue(_currentScript, EditorGUILayout.RectIntField((RectInt)value));
		}
		else if (field.FieldType == typeof(Rect))
		{
			field.SetValue(_currentScript, EditorGUILayout.RectField((Rect)value));
		}
		else //if((typeof(System.SerializableAttribute)) != null)
		{

			var r = field.FieldType.CustomAttributes;
			//var property = _currentComponentSer.FindProperty(field.Name);
			//EditorGUILayout.PropertyField(property);
			EditorGUILayout.LabelField("Cannot print this");
		}

	}

	private void PrintEnum(FieldInfo field)
	{
		field.SetValue(_currentScript, EditorGUILayout.EnumPopup(field.GetValue(_currentScript) as Enum));
	}

	private void PrintObject(FieldInfo field)
	{
		if (field.FieldType.IsSubclassOf(typeof(Object)))
		{
			field.SetValue(_currentScript, EditorGUILayout.ObjectField(field.GetValue(_currentScript) as Object, field.FieldType, true));
		}
		else if (field.FieldType == typeof(AnimationCurve))
		{
			field.SetValue(_currentScript, EditorGUILayout.CurveField(field.GetValue(_currentScript) as AnimationCurve));
		}
		else if (field.FieldType == typeof(string))
		{
			field.SetValue(_currentScript, EditorGUILayout.TextField(field.GetValue(_currentScript) as string));
		}
		else EditorGUILayout.LabelField("Класс не поддерживаeтся");
	}

	private void OnDisable()
    {
		Selection.selectionChanged -= Repaint;
	}

    private void OnGUI()
    {
        PrintScripts();
    }
}

