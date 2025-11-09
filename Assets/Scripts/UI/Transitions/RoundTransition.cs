using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundTransition : UIBase
{
    [SerializeField] private TextMeshProUGUI weekTxt;
    [SerializeField] private TextMeshProUGUI roundTxt;
    private int round = 0;

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
        SoundManager.Instance.PlaySFX(ESFX.Stage_Transition);

        StoryManager.Instance.roundTransitionDone = false;
        Time.timeScale = 0f;
        round = param.Length > 0 && param[0] is int ? (int)param[0] : 1;
        weekTxt.text = weekFormat + round;
        roundTxt.text = roundFormat.ContainsKey(round) ? roundFormat[round] : "Round " + round;
            
        SaveManager.Instance.SetSaveData(nameof(SaveData.stageIdx), round); //round num
        SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 0); //0이어야 함
        SaveManager.Instance.SaveSlot();
    }
    public override void Closed(object[] param)
    {
        GameEventBus.Raise(new TransitionEvents(round));
        GameEventBus.Raise(new TransitionEndEvent(round));
    }
    public void OnTransitionEnd()
    {
        Time.timeScale = 1f;
        UIManager.HideTransition();
    }
}