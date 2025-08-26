using UnityEngine;

public class ContinuePanelUI : UIBase
{
    public void OnClickContinueExitBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Hide();
    }
}
