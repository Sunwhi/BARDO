using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [Header("Stage4")]
    [SerializeField] private Transform[] destinationPos;

    [Header("Stage5")]
    [SerializeField] private Transform stopPos;

    [SerializeField] private VideoController videoController;
    public bool IsFinal { get; private set; } = false;
    private static readonly int[] pattern = { 4, 6, 3 };

   
    public void Teleport(int destId)
    {        
        //이번 도착지가 유효 루트인지 확인.
        PushDest(destId);

        // 최종 루트 완성 시
        if (IsFinal)
        {
            FinalMove();
            return;
        }

        // 일반 텔레포트
        int idx = destId - 1;
        if (idx < 0 || idx >= destinationPos.Length) return;

        var player = StoryManager.Instance.Player;
        player.transform.position = destinationPos[idx].position;
    }

    private void PushDest(int destId)
    {
        var save = SaveManager.Instance.MySaveData;

        if (save.teleportIdxs == null)
        {
            save.teleportIdxs = new List<int>();
        }
        List<int> curDest = save.teleportIdxs;

        // 패턴 길이 초과 또는 기대 값 불일치 시 리셋
        if (curDest.Count >= pattern.Length || destId != pattern[curDest.Count])
        {
            curDest.Clear();
            IsFinal = false;
        }
        else
        {
            curDest.Add(destId);
            IsFinal = (curDest.Count == pattern.Length);
        }

        SaveManager.Instance.SetSaveData(nameof(SaveData.teleportIdxs), curDest);
        SaveManager.Instance.SaveSlot();
    }

    public void FinalMove()
    {
        StoryManager.Instance.Player.playerInput.enabled = false;
        //StartCoroutine(StoryManager.Instance.PlayerWalkByPos(stopPos.position.x));
        videoController.PlayVideo(VideoType.Ending);
        QuestManager.Instance.ClearSubQuest(0);
        gameObject.SetActive(false);
    }
}