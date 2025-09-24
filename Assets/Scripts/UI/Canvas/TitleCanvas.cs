using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TitleCanvas : UICanvas
{
    protected override void Start()
    {
        base.Start();
        ContinueManager.Instance.loadedByContinue = false;
        Time.timeScale = 1f;
    }

    public void OnClickNewGameBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        // 남은 saveslot이 없다면
        if(!SaveManager.Instance.FindEmptySlot())
        {
            UIManager.Show<YesNoPanel>(
            EYesNoPanelType.New,
            new UnityAction(() =>
            {
                // 가장 오래된 슬롯을 덮어쓸 대상으로 지정
                SaveManager.Instance.OldestSaveSlot();
                // 해당 슬롯의 데이터를 새 게임용으로 초기화
                SaveManager.Instance.InitCurSlotAsNewGame();
                MySceneManager.Instance.LoadScene(ESceneType.MainScene);
            })
            );
        }
        else
        {
            // 찾은 빈 솔롯에 새 게임 데이터를 초기화
            SaveManager.Instance.InitCurSlotAsNewGame();
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
        /*if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance is null!");
        }*/
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
