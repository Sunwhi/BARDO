using UnityEngine;
using Newtonsoft.Json;
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
    Slot3 = 3,
    Slot4 = 4,
    Slot5 = 5
}

public class SaveManager : Singleton<SaveManager>
{
    private SaveData saveData;
    public SaveData MySaveData { get { return saveData; } }
    private readonly Dictionary<string, FieldInfo> fieldCache = new();

    private bool isAutoDirty;
    private string directory;

    private string[] paths = new string[5];

    private void Start()
    {
        Init();
        paths[0] = GetSlotPath(ESaveSlot.Slot1);
        paths[1] = GetSlotPath(ESaveSlot.Slot2);
        paths[2] = GetSlotPath(ESaveSlot.Slot3);
        paths[3] = GetSlotPath(ESaveSlot.Slot4);
        paths[4] = GetSlotPath(ESaveSlot.Slot5);
    }

    #region Unity Life Cycles

    public void Init()
    {
        // Save 디렉토리 생성
        directory = Path.Combine(Application.persistentDataPath, "Save");

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        // SaveData 필드를 fieldCache 딕셔너리에 캐싱
        var fields = typeof(SaveData).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (!fieldCache.ContainsKey(field.Name))
            {
                fieldCache.Add(field.Name, field);
            }
        }

        isAutoDirty = false;
        Debug.Log($"[SaveManager] Save directory path: {directory}");
    }

    private void OnApplicationQuit()
    {
        try
        {
            SaveSlot(0); // 자동저장 슬롯
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveManager] 종료 저장 실패: {e.Message}");
        }
    }
    #endregion

    #region Main Methods
    public void CreateSaveData()
    {
        saveData = new(); // 새로운 SaveData 인스턴스 생성, 빈 저장 데이터 생성

        string path = GetSlotPath(0); // 자동저장 슬롯
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
            Debug.LogWarning($"파일 없음: {path}");
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
            Debug.LogError($"로딩 실패: {e.Message}");
            saveData = new SaveData();
        }
    }

    /// <summary> 저장 데이터 할당 </summary>
    /// <param name="field">SaveData의 필드명</param>
    /// <param name="value">field의 자료형에 해당하는 덮어쓰기 값</param>
    /// <param name="indexOrKey"></param>
    public void SetSaveData(string field, object value, object indexOrKey = null)
    {
        if (!fieldCache.TryGetValue(field, out var fieldInfo))
        {
            Debug.LogError($"{field} 필드를 찾을 수 없습니다.");
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
                Debug.LogError($"인덱스 범위 초과: {idx}");
                return;
            }
        }
        else if (fieldValue is IDictionary dict)
        {
            dict[indexOrKey] = value;
        }
        else
        {
            Debug.LogError($"컬렉션이 아닌 필드에 indexOrKey를 사용할 수 없습니다.");
            return;
        }

        isAutoDirty = true;
        //EventManager.Instance.InvokeSaveDataChanged(field);
    }

    #endregion

    #region Sub Methods
    private string GetSlotPath(ESaveSlot slot)
    {
        return Path.Combine(directory, $"{slot}.json");
    }

    // 비어있는 Slot들 중에 가장 첫번째 Slot index를 반환, 자동저장에 사용
    public int FirstEmptySlot()
    {
        if (!File.Exists(paths[0])) return 1;
        if (!File.Exists(paths[1])) return 2;
        if (!File.Exists(paths[2])) return 3;
        if (!File.Exists(paths[3])) return 4;
        if (!File.Exists(paths[4])) return 5;

        return 0;
    }

    public bool HasSaveSlot(ESaveSlot slot)
    {
        if (File.Exists(GetSlotPath(slot))) return true;
        return false;
    }
    #endregion
}