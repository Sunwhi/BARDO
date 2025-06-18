using UnityEngine;

public class TitleSceneUI : MonoBehaviour
{
    public void OnClickStart()
    {
        Debug.Log(MySceneManager.Instance == null ? "인스턴스가 null" : "인스턴스 살아있음");

        MySceneManager.Instance.LoadScene(SceneType.Prototype);
    }
}
