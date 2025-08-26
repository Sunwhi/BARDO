using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    Checkpoint,
    Tutorial,
    Dialogue
}

public class TriggerManager : Singleton<TriggerManager>
{
    private Dictionary<string, Vector3> checkpointTable = new();
    private string lastCheckpointID;

    // 현재 유효한 체크포인트 ID 반환
    public string GetLastCheckpointID()
    {
        return lastCheckpointID;
    }

    // 저장된 ID로 체크포인트 복원
    public void SetLastCheckpointID(string id)
    {
        if (checkpointTable.ContainsKey(id))
            lastCheckpointID = id;
    }

    // 위치 등록
    public void RegisterCheckpoint(string id, Vector3 position)
    {
        if (!checkpointTable.ContainsKey(id))
        {
            checkpointTable.Add(id, position);
        }
        else
        {
            checkpointTable[id] = position;
        }

        SaveManager.Instance.SetSaveData("savedPosition", new SerializableVector3(position)); // SaveData에 savedPosition저장
        lastCheckpointID = id;
    }

    // 리스폰 위치 반환
    public Vector3 GetRespawnPosition()
    {
        if (checkpointTable.TryGetValue(lastCheckpointID, out Vector3 pos))
            return pos;

        Debug.LogWarning("리스폰 위치가 없습니다. 디폴트 (0,0,0) 반환.");
        return Vector3.zero;
    }

    // 트리거에 따른 공통 처리 (튜토리얼, 대화 등 확장 가능)
    public void HandleTrigger(string triggerID, TriggerType type)
    {
        switch (type)
        {
            case TriggerType.Tutorial:
                GameEventBus.Raise(new TutorialEvent(triggerID));
                break;
            case TriggerType.Dialogue:
                break;
            case TriggerType.Checkpoint:
                SaveManager.Instance.SaveSlot((ESaveSlot)SaveManager.Instance.currentSaveSlot); // 자동 저장
                GameEventBus.Raise(new CheckPointEvent(triggerID));
                break;
        }
    }
}