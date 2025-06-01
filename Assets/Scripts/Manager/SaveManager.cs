using UnityEngine;
//using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public enum ESaveSlot
{
    Auto = 0,
    Slot1 = 1,
    Slot2 = 2,
    Slot3 = 3
}

public class SaveManager : Singleton<SaveManager>
{
    /*//private SaveData saveData;
    //public SaveData MySaveData { get { return saveData; } }
    private readonly Dictionary<string, FieldInfo> fieldCache = new();

    private bool isAutoDirty;
    private string directory;

    #region Unity Life Cycles
    public void Init()
    {
        directory = Path.Combine(Application.persistentDataPath, "Save");

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        var fields = typeof(SaveData).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (!fieldCache.ContainsKey(field.Name))
            {
                fieldCache.Add(field.Name, field);
            }
        }

        isAutoDirty = false;
    }

    private void OnApplicationQuit()
    {
        try
        {
            SaveSlot(0);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveManager] ���� ���� ����: {e.Message}");
        }
    }
    #endregion

    #region Main Methods
    public void CreateSaveData()
    {
        saveData = new();

        // �⺻ ���� �߰�
        saveData.ownedRecipes.Add(1);

        string path = GetSlotPath(0);
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public void SaveSlot(ESaveSlot slot)
    {
        if (slot == 0 && !isAutoDirty) return;
        SetSaveData(nameof(SaveData.lastSaveTime), DateTime.Now.Ticks);
        string path = GetSlotPath(slot);
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public void LoadSlot(ESaveSlot slot)
    {
        string path = GetSlotPath(slot);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"���� ����: {path}");
            saveData = new SaveData();
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            saveData = JsonConvert.DeserializeObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"�ε� ����: {e.Message}");
            saveData = new SaveData();
        }
    }

    /// <summary> ���� ������ �Ҵ� </summary>
    /// <param name="field">SaveData�� �ʵ��</param>
    /// <param name="value">field�� �ڷ����� �ش��ϴ� ����� ��</param>
    /// <param name="indexOrKey"></param>
    public void SetSaveData(string field, object value, object indexOrKey = null)
    {
        if (!fieldCache.TryGetValue(field, out var fieldInfo))
        {
            Debug.LogError($"{field} �ʵ带 ã�� �� �����ϴ�.");
            return;
        }

        var fieldValue = fieldInfo.GetValue(saveData);

        if (indexOrKey == null)
        {
            fieldInfo.SetValue(saveData, value);
        }
        else if (fieldValue is IList list && indexOrKey is int idx)
        {
            if (idx >= 0 && idx < list.Count)
            {
                list[idx] = value;
            }
            else
            {
                Debug.LogError($"�ε��� ���� �ʰ�: {idx}");
                return;
            }
        }
        else if (fieldValue is IDictionary dict)
        {
            dict[indexOrKey] = value;
        }
        else
        {
            Debug.LogError($"�÷����� �ƴ� �ʵ忡 indexOrKey�� ����� �� �����ϴ�.");
            return;
        }

        isAutoDirty = true;
        EventManager.Instance.InvokeSaveDataChanged(field);
    }

    #endregion

    #region Sub Methods
    private string GetSlotPath(ESaveSlot slot)
    {
        return Path.Combine(directory, $"{slot}.json");
    }

    #endregion*/
}