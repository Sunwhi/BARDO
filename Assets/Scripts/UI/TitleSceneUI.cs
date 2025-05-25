using UnityEngine;

public class TitleSceneUI : MonoBehaviour
{
    public void OnClickStart()
    {
        MySceneManager.Instance.LoadScene(SceneType.Stage1);
    }
}
