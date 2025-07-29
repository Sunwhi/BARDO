using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePanel : MonoBehaviour
{
    public void OnClickSaveExitBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("SavePanel");
        UIManager.Instance.ShowPanel("EscPanel");
    }
}
