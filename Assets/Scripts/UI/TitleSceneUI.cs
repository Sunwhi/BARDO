using UnityEngine;

public class TitleSceneUI : MonoBehaviour
{
    public void OnClickStart()
    {
        Debug.Log(MySceneManager.Instance == null ? "�ν��Ͻ��� null" : "�ν��Ͻ� �������");

        MySceneManager.Instance.LoadScene(SceneType.Prototype);
    }
}
