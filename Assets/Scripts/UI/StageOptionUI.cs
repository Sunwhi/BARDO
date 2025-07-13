using UnityEngine;
using UnityEngine.SceneManagement;
public class StageOptionUI : MonoBehaviour
{
    private void Start()
    {
        //GameObject optionPanel = this.gameObject;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "TitleScene")
        {
            SoundManager.Instance.PlaySFX(eSFX.UI_Btn_Open_Settings);
            if (UIManager.Instance.IsPanelActive("OptionPanel")) UIManager.Instance.HidePanel("OptionPanel");
            else UIManager.Instance.ShowPanel("OptionPanel");
        }
    }
    
    public void OnClickContinue()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("OptionPanel");
    }

    public void OnClickTItle()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        MySceneManager.Instance.LoadScene(SceneType.Title);
    }

    public void OnClickOptionExit()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("OptionPanel");
    }
}
