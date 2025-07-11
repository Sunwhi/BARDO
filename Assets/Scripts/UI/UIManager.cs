using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/*
 * UIManager
 * uiPanels�� ���� ���� ��� panel���� �����ϰ�,
 * ShowPanel, HidePanel, HideAllPanels�� ���� panel���� �����Ѵ�.
 */
public class UIManager : Singleton<UIManager>
{
    // �гε��� ��� ��ųʸ�
    private Dictionary<string, GameObject> uiPanels = new Dictionary<string, GameObject>();

    public Fadeview fadeView;

    private void Update()
    {
        // TitleScene���� Prototype Scene �Ѿ�� UIManager�� fadeview �ֱ�
        if (fadeView == null)
        {
            fadeView = FindAnyObjectByType<Fadeview>();
        }
    }

    // �Ķ���ͷ� ���� �г��� uiPanels�� ��ųʸ��� �߰��Ѵ�.
    public void RegisterPanels(GameObject panel)
    {
        uiPanels[panel.name] = panel;
    }
    // Panel�� ��Ÿ���� �Լ�
    public void ShowPanel(string panelName)
    {
        if(uiPanels.TryGetValue(panelName, out var panel))
        {
            if(!panel.activeSelf)    panel.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"[UIPanel] : {panelName}�г��� ã�� �� �����ϴ�");
        }
    }

    // Panel�� ����� �Լ�
    public void HidePanel(string panelName)
    {
        if(uiPanels.TryGetValue(panelName, out var panel))
        {
            if(panel.activeSelf)    panel.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"[UIPanel] : {panelName}�г��� ã�� �� �����ϴ�");
        }
    }
    // ���� ���� ��� �г��� ����� �Լ�
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
