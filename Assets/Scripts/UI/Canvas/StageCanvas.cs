using UnityEngine;

public class StageCanvas : UICanvas
{
    void Update()
    {
        if (UIManager.Instance.ActiveStacks[eUIPosition.Popup].Count == 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Btn_Open_Settings);
            UIManager.Show<PausePanel>();
        }
    }
}