using UnityEngine;

public class MapHintPanel : UIBase
{
    public override void Opened(object[] param)
    {
        Time.timeScale = 0.0f;
        base.Opened(param);
    }
    public override void Closed(object[] param)
    {
        Time.timeScale = 1.0f;
        base.Closed(param);
    }
    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        base.OnUICloseBtn();
    }
}
