using UnityEngine;

public class CreditPanel : UIBase
{
    private GameObject blurImg;

    private void Awake()
    {
        opened = OnPanelOpened;
    }
    public override void OnUICloseBtn()
    {
        blurImg.SetActive(false);

        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        base.OnUICloseBtn();
    }
    private void OnPanelOpened(object[] parameters)
    {
        if(parameters.Length > 0 && parameters != null)
        {
            if (parameters[0] is GameObject blurImg)
            {
                this.blurImg = blurImg;
            }
        }
    }
}