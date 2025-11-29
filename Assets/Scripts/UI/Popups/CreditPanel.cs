using UnityEngine;

public class CreditPanel : UIBase
{
    private GameObject blurImg;

    private void Awake()
    {
        base.Awake();
        opened += OnPanelOpened;
        closed += OnPanelClosed;
    }
    public override void OnUICloseBtn()
    {
        if(blurImg != null) blurImg.SetActive(false);

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
    private void OnPanelClosed(object[] parameters)
    {
        if(blurImg != null) 
            blurImg?.SetActive(false);
    }
}