using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class DataManager : Singleton<DataManager>
{
    private readonly Dictionary<string, string> dataDics = new(); //Resources/Json���κ��� �ڵ��ε�.

    #region Unity Life Cycles
    public void Init()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Json");
        foreach (var jsonFile in jsonFiles)
        {
            string fileName = jsonFile.name;
            string jsonData = jsonFile.text;
            dataDics[fileName] = jsonData;
        }
    }
    #endregion

    #region Main Methods
    /// <summary><typeparamref name="T"/>�� �����͸� object�� parsing �� ��ȯ</summary>
    public T GetObj<T>(string key)
    {
        if (dataDics.TryGetValue(key, out var json))
        {
            return ConvertToObj<T>(json, key);
        }
        else
        {
            Debug.LogWarning($"{key}�� DataDics�� �������� �ʽ��ϴ�.");
            return default;
        }
    }

    /// <summary><typeparamref name="T"/>�� �����͸� object�� parsing �� ��ȯ</summary>
    public List<T> GetObjList<T>(string key)
    {
        if (dataDics.TryGetValue(key, out var json))
        {
            return ConvertToList<T>(json, key);
        }
        else
        {
            Debug.LogWarning($"{key}�� DataDics�� �������� �ʽ��ϴ�.");
            return null;
        }
    }

    /// <summary>�����͸� string���� ��ȯ</summary>
    public string GetRawDataList(string key)
    {
        if (dataDics.TryGetValue(key, out var json))
        {
            return json;
        }
        else
        {
            Debug.LogWarning($"{key}�� DataDics�� �������� �ʽ��ϴ�.");
            return null;
        }
    }
    #endregion

    #region Sub Methods
    private T ConvertToObj<T>(string json, string key)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"'{key}'�� JSON ������ ��ȯ �� ���� �߻�: {ex.Message}");
            return default;
        }
    }

    private List<T> ConvertToList<T>(string jsonData, string keyName)
    {
        try
        {
            return JsonConvert.DeserializeObject<List<T>>(jsonData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"'{keyName}'�� JSON �� List<{typeof(T).Name}> ��ȯ ����: {ex.Message}");
            return null;
        }
    }
    #endregion
}