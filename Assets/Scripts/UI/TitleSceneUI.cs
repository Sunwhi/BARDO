using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
        ContinueManager.Instance.loadedByContinue = false;

        // 남은 saveslot이 없다면
        if(SaveManager.Instance.FirstEmptySlot() == 0)
        {
            UIManager.Instance.ShowPanelWithParam<YesNoPanel>(
            EYesNoPanelType.New,
            new UnityAction(() =>
            {
                SaveManager.Instance.CreateSaveData();
                SaveManager.Instance.currentSaveSlot = SaveManager.Instance.OldestSaveSlot();
                //Debug.Log(SaveManager.Instance.OldestSaveSlot());
                MySceneManager.Instance.LoadScene(SceneType.MainScene);
            })
            );
        }
        else
        {
            SaveManager.Instance.CreateSaveData();
            SaveManager.Instance.currentSaveSlot = SaveManager.Instance.FirstEmptySlot(); // 자동 저장할 슬롯 지정, 비어있는 가장 첫번째 슬롯
            MySceneManager.Instance.LoadScene(SceneType.MainScene);
        }
    }

    public void OnClickContinueBtn()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager.Instance is null! 씬에 SoundManager가 없거나 아직 초기화되지 않았습니다.");
        }
        //SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is null!");
        }
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager.Instance is null!");
        }
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance is null!");
        }
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
