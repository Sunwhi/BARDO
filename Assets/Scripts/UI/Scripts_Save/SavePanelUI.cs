using UnityEngine;

public class SavePanelUI : UIBase
{
    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        base.OnUICloseBtn();
    }
}
