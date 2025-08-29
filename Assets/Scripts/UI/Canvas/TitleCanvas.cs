using UnityEngine;
using UnityEngine.Events;

public class TitleCanvas : UICanvas
{
    protected override void Start()
    {
        base.Start();
        ContinueManager.Instance.loadedByContinue = false;
    }

    public void OnClickNewGameBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);

        // 남은 saveslot이 없다면
        if(SaveManager.Instance.FirstEmptySlot() == 0)
        {
            UIManager.Show<YesNoPanel>(
            EYesNoPanelType.New,
            new UnityAction(() =>
            {
                SaveManager.Instance.CreateSaveData();
                SaveManager.Instance.currentSaveSlot = SaveManager.Instance.OldestSaveSlot();
                MySceneManager.Instance.LoadScene(ESceneType.MainScene);
                //SaveManager.Instance.saveSlots[SaveManager.Instance.currentSaveSlot] = 
            })
            );
        }
        else
        {
            SaveManager.Instance.CreateSaveData();
            SaveManager.Instance.currentSaveSlot = SaveManager.Instance.FirstEmptySlot(); // 자동 저장할 슬롯 지정, 비어있는 가장 첫번째 슬롯
            MySceneManager.Instance.LoadScene(ESceneType.MainScene);
        }
    }

    public void OnClickContinueBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);

        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager.Instance is null! 씬에 SoundManager가 없거나 아직 초기화되지 않았습니다.");
        }
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
        UIManager.Show<ContinuePanel>();
    }

    public void OnClickOptionBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Show<SettingPanel>();
    }

    public void OnClickCreditBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Show<CreditPanel>();
    }

    public void OnClickGameExitBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void OnClickExitPanelBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        UIManager.Instance.HideAllPanels();
    }
}
