using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> panelsToRegister;

    private void Start()
    {
        // uiPanel의 모든 패널들이 Clear되었을 때 register.
        if (UIManager.Instance.okToRegisterPanels)
        {
            foreach (var panel in panelsToRegister)
            {
                UIManager.Instance.RegisterPanels(panel);
            }
        }
        UIManager.Instance.okToRegisterPanels = false;
    }

    public void OnClickNewGameBtn()
    {
        //bug.Log(MySceneManager.Instance == null ? "인스턴스가 null" : "인스턴스 살아있음");
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        MySceneManager.Instance.LoadScene(SceneType.MainScene);

        SaveManager.Instance.CreateSaveData();
        SaveManager.Instance.currentSaveSlot = SaveManager.Instance.FirstEmptySlot(); // 자동 저장할 슬롯 지정
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
