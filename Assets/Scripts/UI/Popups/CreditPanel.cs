using UnityEngine;

public class CreditPanel : UIBase
{
    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        base.OnUICloseBtn();
    }
}