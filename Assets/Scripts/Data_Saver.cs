using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class SerializableDictionary<Tkey, Tvalue>
{
    public List<Tkey> Keys = new List<Tkey>();
    public List<Tvalue> Values = new List<Tvalue>();

    public Dictionary<Tkey, Tvalue> ToDictionary()
    {
        Dictionary<Tkey, Tvalue> dictionary = new Dictionary<Tkey, Tvalue>();

        for (int i = 0; i < Keys.Count; i++) 
        { 
            dictionary[Keys[i]] = Values[i]; 
        }

        return dictionary;
    }

    public static SerializableDictionary<Tkey, Tvalue> FromDictionary(Dictionary<Tkey, Tvalue> dictionary)
    {
        SerializableDictionary<Tkey, Tvalue> wrapper = new SerializableDictionary<Tkey, Tvalue>();

        foreach(var kvp in dictionary)
        {
            wrapper.Keys.Add(kvp.Key);
            wrapper.Values.Add(kvp.Value);
        }

        return wrapper;
    }
}
public class Data_Saver : MonoBehaviour
{
    string path;
    public Dictionary<string, bool> openLevels;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "levels.json");
        if (!File.Exists(path))
        {
            openLevels = new Dictionary<string, bool>();
        }
        else
        {
            openLevels = Load();
        }
    }

    public Dictionary<string , bool> Load()
    {
        string json = File.ReadAllText(path);
        SerializableDictionary<string, bool> wrapper = JsonUtility.FromJson<SerializableDictionary<string, bool>>(json);

        return wrapper.ToDictionary();
    }

    public void Save(Dictionary<string, bool> dictionary)
    {
        SerializableDictionary<string, bool> wrapper = SerializableDictionary<string, bool>.FromDictionary(dictionary);
        string json = JsonUtility.ToJson(dictionary, true);
        File.WriteAllText(path, json);
    }

    public void Add(string key, bool value)
    {
        openLevels.Add(key, value);
    }

    public Dictionary<string, bool> Get()
    {
        return openLevels;
    }
}
