using UnityEngine.Events;

public class PausePanel : UIBase
{
    public void OnContinueClicked()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Hide();
        GameEventBus.Raise<PauseGameEvent>(new PauseGameEvent(GameState.resume));
    }

    public void OnSaveClicked()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Show<SavePanel>();
    }

    public void OnMainmenuClicked()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);

        UIManager.Show<YesNoPanel>(
        EYesNoPanelType.Quit,
        new UnityAction(() =>
        {
            MySceneManager.Instance.LoadScene(ESceneType.Title);
            SoundManager.Instance.PlayBGM(EBGM.Title);
        })
        );
    }

    public void OnSettingClicked()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Show<SettingPanel>();
    }

    public void OnClickQuit()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);

        UIManager.Show<YesNoPanel>(
        EYesNoPanelType.Quit,
        new UnityAction(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }));
    }
}
