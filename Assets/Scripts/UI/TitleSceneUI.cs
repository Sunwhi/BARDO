using UnityEngine;

public class TitleSceneUI : MonoBehaviour
{
    public void OnClickStart()
    {
        //bug.Log(MySceneManager.Instance == null ? "인스턴스가 null" : "인스턴스 살아있음");
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        MySceneManager.Instance.LoadScene(SceneType.Prototype);
    }
}
