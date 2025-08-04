using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * UIManager
 * uiPanels에 게임 내의 모든 panel들을 저장하고,
 * ShowPanel, HidePanel, HideAllPanels를 통해 panel들을 관리한다.
 */
public class UIManager : Singleton<UIManager>
{

    // 패널들을 담는 딕셔너리
    public Dictionary<string, GameObject> uiPanels = new Dictionary<string, GameObject>();

    public Fadeview fadeView;

    // 전 씬의 패널들을 모두 삭제한 뒤 새로운 씬의 패널들을 register하기 위한 변수
    public bool okToRegisterPanels = false; 

    private void Update()
    {
        // TitleScene에서 Prototype Scene 넘어갈때 UIManager에 fadeview 넣기
        if (fadeView == null)
        {
            fadeView = FindAnyObjectByType<Fadeview>();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 새로운 씬이 로드되면 uiPanel의 모든 오브젝트들을 삭제 (그리고 다른 Panel 클래스의 Start에서 해당 씬의 패널들을 register한다.)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiPanels.Clear();
        okToRegisterPanels = true;
    }

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

    //사용법 가이드
    //UIManager.Instance.ShowPanelWithParam("PanelName", 변수1, 변수2, ...);
    //변수1부터는 원하는 아무 변수를 원하는 개수만큼.
    //YesNoPanel = EYesNoPanelType, UnityAction yesAction, UnityAction noAction
    public void ShowPanelWithParam(string panelName, object[] param)
    {
        if (uiPanels.TryGetValue(panelName, out var panel))
        {
            if (!panel.activeSelf) panel.SetActive(true);
            UIBase uiBase = panel.GetComponent<UIBase>();
            if (uiBase != null)
            {
                uiBase.opened?.Invoke(param);
            }
        }
        else
        {
            Debug.LogWarning($"[UIPanel] : {panelName}패널을 찾을 수 없습니다");
        }
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

    public bool IsPanelActive(string panelName)
    {
        if (uiPanels.TryGetValue(panelName, out var panel))
        {
            if (panel.activeSelf) return true;
            else return false;
        }
        else return false;
    }
}
