using UnityEngine;

public class StageCanvas : UICanvas
{
    void Update()
    {
        if (UIManager.Instance.ActiveStacks.Count == 0 && Input.GetKeyUp(KeyCode.Escape) && !UIManager.EscDelay)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Btn_Open_Settings);
            UIManager.Show<PausePanel>();
            GameEventBus.Raise<PauseGameEvent>(new PauseGameEvent(GameState.pause));
        }
    }
}