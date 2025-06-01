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

    // ���� ��ȿ�� üũ����Ʈ ID ��ȯ
    public string GetLastCheckpointID()
    {
        return lastCheckpointID;
    }

    // ����� ID�� üũ����Ʈ ����
    public void SetLastCheckpointID(string id)
    {
        if (checkpointTable.ContainsKey(id))
            lastCheckpointID = id;
    }

    // ��ġ ���
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

        lastCheckpointID = id;
    }

    // ������ ��ġ ��ȯ
    public Vector3 GetRespawnPosition()
    {
        if (checkpointTable.TryGetValue(lastCheckpointID, out Vector3 pos))
            return pos;

        Debug.LogWarning("������ ��ġ�� �����ϴ�. ����Ʈ (0,0,0) ��ȯ.");
        return Vector3.zero;
    }

    // Ʈ���ſ� ���� ���� ó�� (Ʃ�丮��, ��ȭ �� Ȯ�� ����)
    public void HandleTrigger(string triggerID, TriggerType type)
    {
        switch (type)
        {
            case TriggerType.Tutorial:
                break;
            case TriggerType.Dialogue:
                break;
            case TriggerType.Checkpoint:
                break;
        }
    }
}