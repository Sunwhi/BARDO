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
    public SaveData MySaveData { get { return saveSlots[currentSaveSlot]; } }
    public SaveData[] saveSlots = new SaveData[5];

    private readonly Dictionary<string, FieldInfo> fieldCache = new();

    private bool isAutoDirty;
    private string directory;

    private string[] slotPaths = new string[5]; // 슬롯들의 경로 path

    public int currentSaveSlot = 0; // 현재 자동 저장되고 있는 슬롯


    private void Start()
    {
        Init();
        string json;
        for (int i=0; i<5; i++)
        {
            json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            Debug.Log(json);
        }
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

        // slotPaths, saveSlots 초기화
        for(int i=0; i<5; i++)
        {
            slotPaths[i] = GetSlotPath(IntToESaveSlot(i+1));

            LoadSlot(IntToESaveSlot((i+1)));
            saveSlots[i] = saveData;
        }

        isAutoDirty = false;
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
    }

    #endregion

    #region Sub Methods

    public ESaveSlot IntToESaveSlot(int slotNum)
    {
        switch(slotNum)
        {
            case 1:
                return ESaveSlot.Slot1;
            case 2:
                return ESaveSlot.Slot2;
            case 3:
                return ESaveSlot.Slot3;
            case 4:
                return ESaveSlot.Slot4;
            case 5:
                return ESaveSlot.Slot5;
        }
        return 0;
    }
    private string GetSlotPath(ESaveSlot slot)
    {
        return Path.Combine(directory, $"{slot}.json");
    }

    // 비어있는 Slot들 중에 가장 첫번째 Slot index를 반환, autosave에 사용
    public int FirstEmptySlot()
    {
        if (!File.Exists(slotPaths[0])) return 1;
        if (!File.Exists(slotPaths[1])) return 2;
        if (!File.Exists(slotPaths[2])) return 3;
        if (!File.Exists(slotPaths[3])) return 4;
        if (!File.Exists(slotPaths[4])) return 5;

        return 0;
    }

    public bool HasSaveSlot(ESaveSlot slot)
    {
        if (File.Exists(GetSlotPath(slot))) return true;
        return false;
    }

    // 가장 오래전에 저장된 슬롯을 반환
    public int OldestSaveSlot()
    {
        int returnSlot = 0;

        long[] oldestSaveSlot = new long[5];

        LoadSlot(ESaveSlot.Slot1);
        oldestSaveSlot[0] = saveData.lastSaveTime;
        LoadSlot(ESaveSlot.Slot2);
        oldestSaveSlot[1] = saveData.lastSaveTime;
        LoadSlot(ESaveSlot.Slot3);
        oldestSaveSlot[2] = saveData.lastSaveTime;
        LoadSlot(ESaveSlot.Slot4);
        oldestSaveSlot[3] = saveData.lastSaveTime;
        LoadSlot(ESaveSlot.Slot5);
        oldestSaveSlot[4] = saveData.lastSaveTime;

        long temp = oldestSaveSlot[0];
        for(int i=0; i<5; i++)
        {
            if (oldestSaveSlot[i] <= temp)
            {
                temp = oldestSaveSlot[i];
                returnSlot = i+1;
            }
        }
        return returnSlot;
    }
    #endregion
}