using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

[Serializable]
public class ReferenceCollectorData
{
    public string key;
    public Object gameObject;
}

public class ReferenceCollectorDataComparer : IComparer<ReferenceCollectorData>
{
    public int Compare(ReferenceCollectorData x, ReferenceCollectorData y)
    {
        return string.Compare(x.key, y.key, StringComparison.Ordinal);
    }
}

public class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
{
    public List<ReferenceCollectorData> data = new List<ReferenceCollectorData>();

    private readonly Dictionary<string, Object> dict = new Dictionary<string, Object>();

#if UNITY_EDITOR
    public void Add(string key, Object obj)
    {
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        for (i = 0; i < data.Count; i++)
        {
            if (data[i].key == key)
            {
                break;
            }
        }
        if (i != data.Count)
        {
            SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }
        else
        {
            dataProperty.InsertArrayElementAtIndex(i);
            SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("key").stringValue = key;
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Remove(string key)
    {
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        for (i = 0; i < data.Count; i++)
        {
            if (data[i].key == key)
            {
                break;
            }
        }
        if (i != data.Count)
        {
            dataProperty.DeleteArrayElementAtIndex(i);
        }
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Clear()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        var dataProperty = serializedObject.FindProperty("data");
        dataProperty.ClearArray();
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Sort()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        data.Sort(new ReferenceCollectorDataComparer());
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }
#endif

    public T Get<T>(string key) where T : class
    {
        Object dictGo;
        if (!dict.TryGetValue(key, out dictGo))
        {
            // throw new Exception("Object reference not set to an instance of an object.(key: " + key + ")");
            Debug.LogError("Object reference not set to an instance of an object. ->key: " + key + ".");
            return null;
        }
        return dictGo as T;
    }

    public T GetComponent<T>(string key) where T : class
    {
        var obj = GetObject(key) as GameObject;
        var component = obj?.GetComponent<T>() ?? Get<T>(key);

        if (component == null)
        {
            Debug.LogError("'" + key + "' Object is not gameObject or no '" + typeof(T) + "' component.");
            return null;
        }
        return component;
    }

    public Object GetObject(string key)
    {
        Object dictGo;
        if (!dict.TryGetValue(key, out dictGo))
        {
            Debug.LogError("Object reference not set to an instance of an object. ->key: " + key + ".");
            return null;
        }
        return dictGo;
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        dict.Clear();
        foreach (ReferenceCollectorData referenceCollectorData in data)
        {
            if (!dict.ContainsKey(referenceCollectorData.key))
            {
                dict.Add(referenceCollectorData.key, referenceCollectorData.gameObject);
            }
        }
    }
}