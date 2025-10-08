using UnityEngine;
using UnityEngine.Events;

public class PausePanel : UIBase
{
    public override void Opened(object[] param)
    {
        Time.timeScale = 0f;
    }

    public override void Closed(object[] param)
    {
        Time.timeScale = 1f;
    }

    public void OnContinueClicked()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Hide(false);
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
            SaveManager.Instance.SaveSlot();
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
