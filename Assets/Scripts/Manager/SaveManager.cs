using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public enum ESaveSlot
{
    Auto = 0,
    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5,
}

public class SaveManager : Singleton<SaveManager>
{
    public SaveData MySaveData 
    { 
        get { return saveCache[curSlot]; } 
        private set { saveCache[curSlot] = value; } 
    }
    private readonly Dictionary<string, FieldInfo> fieldCache = new();
    private Dictionary<int, SaveData> saveCache = new();
    private int curSlot = 0;

    private bool isDirty;
    private string directory;

    #region Unity Life Cycles
    public void Init()
    {
        //������ ���ϰ�� ����
        directory = Path.Combine(Application.persistentDataPath, "Save");
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        isDirty = false;

        //SaveData Field Cache
        var fields = typeof(SaveData).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (!fieldCache.ContainsKey(field.Name))
            {
                fieldCache.Add(field.Name, field);
            }
        }

        bool isAutoCache = true; //�ڵ� ���� ������ �ٸ� ���԰� �ٸ� ��쿡�� true�� ����

        for (int i = 0; i < 6; i++)
        {
            ESaveSlot slot = (ESaveSlot)i;
            string path = GetSlotPath(slot);

            if (!File.Exists(path))
            {
                SaveData slotData = CreateSaveData(slot);
                File.WriteAllText(path, JsonConvert.SerializeObject(slotData, Formatting.Indented));
                saveCache[(int)slot] = slotData;
            }
            else
            {
                saveCache[(int)slot] = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(path));
            }

            if (isAutoCache && slot != ESaveSlot.Auto)
            {
                isAutoCache = !(saveCache[0].lastSaveTime == saveCache[i].lastSaveTime);
            }
        }

        if (isAutoCache)
        {
            //TODO : ���� ���� UI ����
        }
    }
    #endregion

    #region Main Methods
    public void SaveSlot(ESaveSlot slot = ESaveSlot.Auto, string slotName = "")
    {
        if (!isDirty) return;

        string path = GetSlotPath(slot);
        if (!File.Exists(path))
        {
            CreateSaveData(slot);
        }

        //Save Slot Name
        SetSaveData(nameof(SaveData.saveName), slotName.IsNullOrWhiteSpace() ? MySaveData.saveName : slotName);
        //Save Time
        SetSaveData(nameof(SaveData.lastSaveTime), DateTime.Now.Ticks);

        string json = JsonConvert.SerializeObject(MySaveData, Formatting.Indented);
        File.WriteAllText(path, json);

        if (slot != ESaveSlot.Auto)
            File.WriteAllText(GetSlotPath(ESaveSlot.Auto), json);
    }

    public void LoadSlot(ESaveSlot slot)
    {
        if (saveCache[(int)slot].lastSaveTime == 0)
        {
            Debug.LogWarning($"����� �����Ͱ� �����ϴ�: {slot}");
            return;
        }

        curSlot = (int)slot;
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

        var fieldValue = fieldInfo.GetValue(MySaveData);

        if (indexOrKey == null)
        {
            fieldInfo.SetValue(MySaveData, value);
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

        isDirty = true;
    }

    #endregion

    #region Sub Methods
    private SaveData CreateSaveData(ESaveSlot slot)
    {
        SaveData saveData = new();
        switch (slot)
        {
            case ESaveSlot.Auto:
                saveData.saveName = "Auto";
                break;
            case ESaveSlot.Slot1:
                saveData.saveName = "Slot1";
                break;
            case ESaveSlot.Slot2:
                saveData.saveName = "Slot2";
                break;
            case ESaveSlot.Slot3:
                saveData.saveName = "Slot3";
                break;
            case ESaveSlot.Slot4:
                saveData.saveName = "Slot4";
                break;
            case ESaveSlot.Slot5:
                saveData.saveName = "Slot5";
                break;
        }

        return saveData;
    }

    private string GetSlotPath(ESaveSlot slot)
    {
        return Path.Combine(directory, $"{slot}.json");
    }
    #endregion
}