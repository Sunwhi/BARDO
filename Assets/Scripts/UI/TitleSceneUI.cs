using UnityEngine;

public class TitleSceneUI : MonoBehaviour
{
    public void OnClickStart()
    {
        //bug.Log(MySceneManager.Instance == null ? "�ν��Ͻ��� null" : "�ν��Ͻ� �������");
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        MySceneManager.Instance.LoadScene(SceneType.Prototype);
    }
}
