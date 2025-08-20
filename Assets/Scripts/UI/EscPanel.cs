using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EscPanel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "TitleScene")
        {
            SoundManager.Instance.PlaySFX(eSFX.UI_Btn_Open_Settings);
            UIManager.Instance.ShowPanel("EscBGImg");
            if (UIManager.Instance.IsPanelActive("EscPanel"))
            {
                UIManager.Instance.HidePanel("EscPanel");
                UIManager.Instance.HidePanel("EscBGImg");
            }
            else UIManager.Instance.ShowPanel("EscPanel");
        }
    }
    public void OnClickContinue()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("EscPanel");
        UIManager.Instance.HidePanel("EscBGImg");
    }

    public void OnClickTItle()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);

        UIManager.Instance.HidePanel("EscBGImg");
        UIManager.Instance.HidePanel("EscPanel");

        UIManager.Instance.ShowPanelWithParam<YesNoPanel>(
        EYesNoPanelType.Quit,
        new UnityAction(() =>
        {
            MySceneManager.Instance.LoadScene(SceneType.Title);
            SoundManager.Instance.PlayBGM(eBGM.Title);
            //ContinueManager.Instance.loadedByContinue = false; // 로드 후 메인메뉴로 돌아왔을 때 초기화해야 InActiveObjects가 비활성화된다.
        }),
        new UnityAction(() =>
        {
            UIManager.Instance.ShowPanel("EscPanel");
            UIManager.Instance.ShowPanel("EscBGImg");
        })
        );
    }

    public void OnClickOption()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("EscPanel");
        UIManager.Instance.ShowPanel("OptionPanel");
    }

    public void OnClickSave()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("EscPanel");
        UIManager.Instance.ShowPanel("SavePanel");
    }

    public void OnClickQuit()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("EscBGImg");
        UIManager.Instance.HidePanel("EscPanel");

        UIManager.Instance.ShowPanelWithParam<YesNoPanel>(
        EYesNoPanelType.Quit,
        new UnityAction(() =>
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
                Application.Quit();
    #endif
        }),
        new UnityAction(() =>
        {
            UIManager.Instance.ShowPanel("EscPanel");
            UIManager.Instance.ShowPanel("EscBGImg");
        })
        );
    }
}
