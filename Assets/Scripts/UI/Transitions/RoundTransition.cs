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
        { 1, "Round 1" },
        { 2, "Round 2" },
        { 3, "Round 3" },
        { 4, "Round 4" },
        { 5, "Round 5" }
    };

    public override void Opened(object[] param)
    {
        Time.timeScale = 0f;
        int round = param.Length > 0 && param[0] is int ? (int)param[0] : 1;
        weekTxt.text = weekFormat + round;
        roundTxt.text = roundFormat.ContainsKey(round) ? roundFormat[round] : "Round " + round;
    }

    public void OnTransitionEnd()
    {
        Time.timeScale = 1f;
        UIManager.HideTransition();
    }
}