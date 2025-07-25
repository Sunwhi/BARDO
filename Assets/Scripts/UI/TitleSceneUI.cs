using System.Collections.Generic;
using UnityEngine;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> panelsToRegister;

    private void Start()
    {
        foreach(var panel in panelsToRegister)
        {
            UIManager.Instance.RegisterPanels(panel);
        }
    }
    public void OnClickNewGameBtn()
    {
        //bug.Log(MySceneManager.Instance == null ? "인스턴스가 null" : "인스턴스 살아있음");
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        MySceneManager.Instance.LoadScene(SceneType.Prototype);
    }

    public void OnClickContinueBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.ShowPanel("ContinuePanel");
    }

    public void OnClickOptionBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.ShowPanel("OptionPanel");
        UIManager.Instance.ShowPanel("EscBGImg");
    }

    public void OnClickCreditBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.ShowPanel("CreditPanel");
    }

    public void OnClickGameExitBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void OnClickExitPanelBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HideAllPanels();
    }
}
