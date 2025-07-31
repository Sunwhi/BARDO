using UnityEngine;

public class ContinuePanelUI : MonoBehaviour
{
    public void OnClickContinueExitBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("ContinuePanel");
    }
}
