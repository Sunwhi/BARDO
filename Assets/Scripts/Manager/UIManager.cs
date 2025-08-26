using UnityEngine;
using System.Collections.Generic;
/*
 * UIManager
 * uiPanels에 게임 내의 모든 panel들을 저장하고,
 * ShowPanel, HidePanel, HideAllPanels를 통해 panel들을 관리한다.
 */
public class UIManager : Singleton<UIManager>
{
    private Transform uiParent;
    private readonly List<UIBase> uiList = new();
    private readonly Stack<UIBase> activeUI = new();

    public Fadeview fadeView;

    private static readonly string uiFolder = "UIs";

    //TODO : 수정.
    private void Update()
    {
        // TitleScene에서 Prototype Scene 넘어갈때 UIManager에 fadeview 넣기
        if (fadeView == null)
        {
            fadeView = FindAnyObjectByType<Fadeview>();
        }

        if (activeUI.Count > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public static void SetCanvas(Transform transform)
    {
        Instance.uiParent = transform;
        Instance.uiList.Clear();
        Instance.activeUI.Clear();
    }

    /// <summary> UI를 생성하고 isActiveInCreated에 따라 활성화하는 메서드 </summary>
    /// <typeparam name="T">UIBase를 상속받은 클래스</typeparam>
    /// <param name="param">원하는 변수를 원하는 개수만큼!</param>
    public static T Show<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).Name;
        var ui = Instance.uiList.Find(obj => obj.name == uiName);

        if (ui == null)
        {
            ui = ResourceManager.Instance.Instantiate<T>(uiFolder, uiName, Instance.uiParent);
            if (ui == null)
            {
                Debug.LogWarning($"[UIManager] : {uiName}을 찾을 수 없습니다");
                return null;
            }
            ui.name = uiName;
            Instance.uiList.Add(ui);
        }

        if (Instance.activeUI.TryPeek(out var top))
        {
            top.gameObject.SetActive(false);
        }

        Instance.activeUI.Push(ui);
        ui.gameObject.SetActive(true);
        ui.opened?.Invoke(param);
        return (T)ui;
    }

    /// <summary> Scene에 생성된 UI 반환 </summary>
    /// <typeparam name="T">UIBase를 상속받은 클래스</typeparam>
    public static T Get<T>() where T : UIBase
    {
        return (T)Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
    }

    /// <summary>
    /// UI를 isDestroyAtClosed에 따라 숨기거나 파괴
    /// </summary>
    /// <param name="param">원하는 변수를 원하는 개수만큼!</param>
    public static void Hide(params object[] param)
    {
        if (Instance.activeUI.Count == 0)
        {
            Debug.LogWarning("[UIManager] : 닫을 UI가 없습니다");
            return;
        }

        var ui = Instance.activeUI.Pop();
        if (ui != null)
        {
            ui.closed.Invoke(param);

            if (ui.isDestroyAtClosed)
            {
                Instance.uiList.Remove(ui);
                Destroy(ui.gameObject);
            }
            else
            {
                ui.gameObject.SetActive(false);
            }

            if (Instance.activeUI.TryPeek(out var top))
            {
                top.gameObject.SetActive(true);
            }
        }
    }

    public void HideAllPanels()
    {
        while (activeUI.Count > 0)
        {
            var ui = activeUI.Pop();
            if (ui == null) continue;

            ui.closed.Invoke(null);
            
            if (ui.isDestroyAtClosed)
            {
                uiList.Remove(ui);
                Destroy(ui.gameObject);
            }
            else
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    public bool IsPanelActive<T>() where T : UIBase
    {
        var panel = uiList.Find(obj => obj.name == typeof(T).Name);

        if (panel == null)
        {
            Debug.LogWarning($"[UIManager] : {typeof(T).Name}을 찾을 수 없습니다");
            return false;
        }

        if (activeUI.Contains(panel)) return true;
        else return false;
    }
}