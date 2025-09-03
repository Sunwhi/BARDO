using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Fadeview fadeView;
    
    private Transform[] uiParents;
    private static readonly string uiFolder = "UIs";

    private readonly Dictionary<Type, UIBase> uiCache = new();
    public Stack<UIBase> ActiveStacks { get; private set; } = new();
    private UIBase activeTransition;

    public static bool EscDelay { get; private set; } = false;

    private void Update()
    {
        if (ActiveStacks.Count > 0 && Input.GetKeyUp(KeyCode.Escape))
        {
            Hide();
            GameEventBus.Raise<PauseGameEvent>(new PauseGameEvent(GameState.resume));
            if (ActiveStacks.Count == 0) StartCoroutine(EscDelayed());
        }
    }

    public static void SetCanvas(Transform[] t)
    {
        Instance.uiParents = t;
        Instance.uiCache.Clear();
        Instance.ActiveStacks.Clear();
        Instance.activeTransition = null;
    }

    /// <summary> UI를 생성하고 isActiveInCreated에 따라 활성화하는 메서드 </summary>
    /// <typeparam name="T">UIBase를 상속받은 클래스</typeparam>
    /// <param name="param">원하는 변수를 원하는 개수만큼!</param>
    public static T Show<T>(params object[] param) where T : UIBase
    {
        var t = typeof(T);
        
        Instance.uiCache.TryGetValue(t, out var ui);
        if (ui == null)
        {
            var prefab = ResourceManager.Instance.LoadAsset<T>(uiFolder, t.Name);
            if (prefab == null)
            {
                Debug.LogError($"[UIManager] {t.Name} 프리팹을 찾을 수 없습니다.");
                return null;
            }

            ui = Instantiate(prefab, Instance.uiParents[(int)prefab.uiPosition]);
            ui.name = t.Name;
            Instance.uiCache[t] = ui;
        }

        switch (ui.uiPosition)
        {
            case eUIPosition.Default:
                break;
            case eUIPosition.Popup:
            case eUIPosition.Override:
                if (Instance.ActiveStacks.TryPeek(out var top))
                {
                    top.gameObject.SetActive(false);
                }
                Instance.ActiveStacks.Push(ui);
                break;
            case eUIPosition.Transition:
                if (Instance.activeTransition)
                {
                    Instance.activeTransition.gameObject.SetActive(false);
                }
                Instance.activeTransition = ui;
                break;
            default:
                Debug.LogWarning("[UIManager] : 정의되지 않은 UI Position입니다");
                return null;
        }

        ui.gameObject.SetActive(true);
        ui.opened?.Invoke(param);
        return (T)ui;
    }

    /// <summary> Scene에 생성된 UI 반환 </summary>
    /// <typeparam name="T">UIBase를 상속받은 클래스</typeparam>
    public static T Get<T>() where T : UIBase
    {
        var t = typeof(T);
        if (Instance.uiCache.TryGetValue(t, out var ui) && ui) return (T)ui;
        return null;
    }

    /// <summary>
    /// UI를 isDestroyAtClosed에 따라 숨기거나 파괴
    /// </summary>
    /// <param name="param">원하는 변수를 원하는 개수만큼!</param>
    public static void Hide(params object[] param)
    {
        if (Instance.activeTransition && Instance.activeTransition.gameObject.activeSelf)
            return;

        TryPopAndClose(Instance.ActiveStacks, Instance, param);
    }

    public static void HideTransition(params object[] param)
    {
        var ui = Instance.activeTransition;
        ui.closed?.Invoke(param);
        if (ui.isDestroyAtClosed)
        {
            Instance.uiCache.Remove(ui.GetType());
            Destroy(ui.gameObject);
        }
        else
        {
            ui.gameObject.SetActive(false);
        }

        Instance.HideAllPanels();
    }

    public void HideAllPanels()
    {
        CloseAllInStack(ActiveStacks);
    }

    public bool IsActive<T>() where T : UIBase
    {
        if (!uiCache.TryGetValue(typeof(T), out var panel) || !panel)
        {
            Debug.LogWarning($"[UIManager] : {typeof(T).Name}을 찾을 수 없습니다");
            return false;
        }

        if (activeTransition == panel) return true;
        if (ActiveStacks.Contains(panel)) return true;
        return false;
    }

    private static bool TryPopAndClose(Stack<UIBase> stack, UIManager inst, object[] param)
    {
        if (!stack.TryPeek(out _)) return false;
        var ui = stack.Pop();
        if (ui)
        {
            ui.closed?.Invoke(param);
            if (ui.isDestroyAtClosed)
            {
                inst.uiCache.Remove(ui.GetType());
                Destroy(ui.gameObject);
            }
            else
            {
                ui.gameObject.SetActive(false);
            }
        }

        if (stack.TryPeek(out var next) && next) next.gameObject.SetActive(true);
        return true;
    }

    private void CloseAllInStack(Stack<UIBase> stack)
    {
        while (stack.TryPeek(out _))
        {
            var ui = stack.Pop();
            if (!ui) continue;

            ui.closed?.Invoke(null);

            if (ui.isDestroyAtClosed)
            {
                uiCache.Remove(ui.GetType());
                Destroy(ui.gameObject);
            }
            else
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    private static IEnumerator EscDelayed()
    {
        EscDelay = true;
        yield return new WaitForSeconds(0.2f);
        EscDelay = false;
    }
}