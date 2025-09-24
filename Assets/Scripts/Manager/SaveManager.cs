using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public enum ESaveSlot
{
    Default = -1,
    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5,
}

public class SaveManager : Singleton<SaveManager>
{
    public event Action<ESaveSlot> OnSaveSlotUpdated; // SaveSlot에 새로운 데이터 업데이트

    public SaveData MySaveData { get { return SaveSlots[curSaveIdx]; } private set { SaveSlots[curSaveIdx] = value; } }
    private int curSaveIdx = 0; // 현재 자동 저장되고 있는 슬롯 : 0~4.
    public SaveData[] SaveSlots { get; private set; } = new SaveData[5];

    private readonly Dictionary<string, FieldInfo> fieldCache = new();

    private string directory;
    private string[] slotPaths = new string[5]; // 슬롯들의 경로 path

    private bool isAutoDirty;

    private void Start()
    {
        Init();
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

        curSaveIdx = 0;

        // slotPaths, saveSlots 초기화
        for (int i = 0; i < 5; i++)
        {
            ESaveSlot slot = (ESaveSlot)i;
            slotPaths[i] = GetSlotPath(slot);
            LoadSlot(slot);
        }

        isAutoDirty = false;
    }

    private void OnApplicationQuit()
    {
        try
        {
            SaveSlot((ESaveSlot)curSaveIdx); // 자동저장 슬롯
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] 종료 저장 실패: {e.Message}");
        }
    }
    #endregion

    #region Main Methods
    public void SaveSlot(ESaveSlot slot = ESaveSlot.Default)
    {
        if (!isAutoDirty) return;
        if (slot == ESaveSlot.Default) slot = (ESaveSlot)curSaveIdx;

        int idx = (int)slot;

        SetSaveData(nameof(SaveData.lastSaveTime), DateTime.Now.Ticks);
        string path = slotPaths[idx];
        string json = JsonConvert.SerializeObject(SaveSlots[idx], Formatting.Indented);
        File.WriteAllText(path, json);

        curSaveIdx = (int)slot;
        OnSaveSlotUpdated?.Invoke(slot);
    }

    public void LoadSlot(ESaveSlot slot)
    {
        string path = slotPaths[(int)slot];
        if (!File.Exists(path))
        {
            SaveSlots[(int)slot] = new SaveData();
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            SaveSlots[(int)slot] = JsonConvert.DeserializeObject<SaveData>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SavManager] {slot} 로딩 실패: \n{e.Message}");
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

        if (!MySaveData.dataSaved) MySaveData.dataSaved = true;
    }

    //현재 선택된 슬롯을 새로운 게임 데이터로 초기화
    public void InitCurSlotAsNewGame()
    {
        SaveSlots[curSaveIdx] = new SaveData();
        SaveSlots[curSaveIdx].dataSaved = true;
        OnSaveSlotUpdated?.Invoke((ESaveSlot)curSaveIdx);
        isAutoDirty = true;
    }

    public bool HasSaveSlot(ESaveSlot slot)
    {
        if (File.Exists(GetSlotPath(slot)) && SaveSlots[(int)slot].dataSaved) return true;
        return false;
    }

    // 비어있는 Slot들 중에 가장 첫번째 Slot index를 반환, autosave에 사용
    public bool FindEmptySlot()
    {
        for (int i = 0; i < 5; i++)
        {
            if (File.Exists(slotPaths[i])) continue;
            curSaveIdx = i;
            return true;
        }

        return false;
    }

    // 가장 오래전에 저장된 슬롯을 반환
    public void OldestSaveSlot()
    {
        int returnSlot = 0;
        long[] oldestSaveSlot = new long[5];

        oldestSaveSlot[0] = SaveSlots[0].lastSaveTime;
        oldestSaveSlot[1] = SaveSlots[1].lastSaveTime;
        oldestSaveSlot[2] = SaveSlots[2].lastSaveTime;
        oldestSaveSlot[3] = SaveSlots[3].lastSaveTime;
        oldestSaveSlot[4] = SaveSlots[4].lastSaveTime;

        long temp = oldestSaveSlot[0];
        for (int i = 1; i < 5; i++)
        {
            if (oldestSaveSlot[i] <= temp)
            {
                temp = oldestSaveSlot[i];
                returnSlot = i;
            }
        }
        curSaveIdx = returnSlot;
    }

    public void SetSaveSlotIdx(int idx)
    {
        curSaveIdx = idx;
    }

    public void copySaveData(int idx)
    {
        string json = JsonUtility.ToJson(MySaveData);
        SaveSlots[idx] = JsonUtility.FromJson<SaveData>(json);
    }
    #endregion

    #region Sub Methods
    private string GetSlotPath(ESaveSlot slot)
    {
        return Path.Combine(directory, $"{slot}.json");
    }
    #endregion
}