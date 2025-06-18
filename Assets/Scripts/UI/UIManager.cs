using System.Collections.Generic;
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
    public void ShowUiPanels()
    {
        foreach (var panel in uiPanels.Values) Debug.Log(panel.name);
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
}
