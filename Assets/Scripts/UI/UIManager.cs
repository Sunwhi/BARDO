using System.Collections.Generic;
using UnityEngine;
/*
 * UIManager
 * uiPanels에 게임 내의 모든 panel들을 저장하고,
 * ShowPanel, HidePanel, HideAllPanels를 통해 panel들을 관리한다.
 */
public class UIManager : Singleton<UIManager>
{
    // 패널들을 담는 딕셔너리
    private Dictionary<string, GameObject> uiPanels = new Dictionary<string, GameObject>();

    public Fadeview fadeView;

    // 파라미터로 받은 패널을 uiPanels의 딕셔너리에 추가한다.
    public void RegisterPanels(GameObject panel)
    {
        uiPanels[panel.name] = panel;
    }
    // Panel을 나타내는 함수
    public void ShowPanel(string panelName)
    {
        if(uiPanels.TryGetValue(panelName, out var panel))
        {
            if(!panel.activeSelf)    panel.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"[UIPanel] : {panelName}패널을 찾을 수 없습니다");
        }
    }
    public void ShowUiPanels()
    {
        foreach (var panel in uiPanels.Values) Debug.Log(panel.name);
    }
    // Panel을 숨기는 함수
    public void HidePanel(string panelName)
    {
        if(uiPanels.TryGetValue(panelName, out var panel))
        {
            if(panel.activeSelf)    panel.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"[UIPanel] : {panelName}패널을 찾을 수 없습니다");
        }
    }
    // 게임 내의 모든 패널을 숨기는 함수
    public void HideAllPanels()
    {
        foreach(var panel in uiPanels.Values)
        {
            if (panel.activeSelf) panel.SetActive(false);
        }
    }
}
