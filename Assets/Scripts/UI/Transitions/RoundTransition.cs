using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundTransition : UIBase
{
    [SerializeField] private TextMeshProUGUI weekTxt;
    [SerializeField] private TextMeshProUGUI roundTxt;

    private readonly string weekFormat = "Week ";
    private readonly Dictionary<int, string> roundFormat = new()
    {
        { 1, "잃어버린 기억" },
        { 2, "풀리지 않는 비밀" },
        { 3, "진실을 향한 여정" },
        { 4, "이상한 세계" },
        { 5, "인연과 운명" },
        { 6, "마지막 선택" },
        { 7, "밝혀지는 비밀" },
    };

    public override void Opened(object[] param)
    {
        StoryManager.Instance.roundTransitionDone = false;
        Time.timeScale = 0f;
        int round = param.Length > 0 && param[0] is int ? (int)param[0] : 1;
        weekTxt.text = weekFormat + round;
        roundTxt.text = roundFormat.ContainsKey(round) ? roundFormat[round] : "Round " + round;

        SaveManager.Instance.SetSaveData(nameof(SaveData.stageIdx), round);
        SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 0);
        SaveManager.Instance.SaveSlot();
    }
    public override void Closed(object[] param)
    {
        GameEventBus.Raise(new TransitionEvents());
    }
    public void OnTransitionEnd()
    {
        Time.timeScale = 1f;
        UIManager.HideTransition();
    }
}