using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TitleCanvas : UICanvas
{
    [SerializeField] TimelineController timelineController;
    [SerializeField] GameObject SkipIntroBtn;
    [SerializeField] GameObject BGBlurImage;

    private bool menuDirectorFin = false;
    private void OnEnable()
    {
        timelineController.OnMenuDirectorFinEvent += HandleMenuDirFin;
    }
    private void OnDisable()
    {
        timelineController.OnMenuDirectorFinEvent -= HandleMenuDirFin;
    }

    protected override void Start()
    {
        base.Start();
        ContinueManager.Instance.loadedByContinue = false;
        Time.timeScale = 1f;
        menuDirectorFin = false;
    }

    public void OnClickNewGameBtn()
    {
        if (menuDirectorFin)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
            BGBlurImage.SetActive(true);

            // 남은 saveslot이 없다면
            if (!SaveManager.Instance.FindEmptySlot())
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

            DialogueManager.Instance.StartNewStory();
        }
    }

    public void OnClickContinueBtn()
    {
        if (menuDirectorFin)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
            BGBlurImage.SetActive(true);

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
    }

    public void OnClickOptionBtn()
    {
        if (menuDirectorFin)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
            BGBlurImage.SetActive(true);

            UIManager.Show<SettingPanel>();
        }
    }

    public void OnClickCreditBtn()
    {
        if (menuDirectorFin)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
            BGBlurImage.SetActive(true);

            UIManager.Show<CreditPanel>();
        }
    }

    public void OnClickGameExitBtn()
    {
        if (menuDirectorFin)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
            SkipIntroBtn.SetActive(false);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
        }
    }

    public void OnClickExitPanelBtn()
    {
        if(menuDirectorFin)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
            BGBlurImage.SetActive(false);
            Debug.Log("falseplease");
            UIManager.Instance.HideAllPanels();
        }
    }

    public void OnClickIntroSkipBtn()
    {
        if (!menuDirectorFin)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
            timelineController.SkipIntro();
            SkipIntroBtn.SetActive(false);
        }
    }

    private void HandleMenuDirFin()
    {
        menuDirectorFin = true;
    }
}
