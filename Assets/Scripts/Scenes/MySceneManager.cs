using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * MySceneManager
 */
public enum SceneType { // Scene�� �̸����� ��� enum ���� -> LoadScene�� ���
    Title, Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, Prototype
}
public class MySceneManager : Singleton<MySceneManager>
{
    // ���� �ε���.
    public void LoadScene(SceneType type)
    {
        switch(type)
        {
            case SceneType.Title:
                SceneManager.LoadScene("TitleScene");
                break;
            case SceneType.Prototype:
                SceneManager.LoadScene("PrototypeTest");
                break;
            case SceneType.Stage1:
                SceneManager.LoadScene("Stage1");
                break;
            case SceneType.Stage2:
                SceneManager.LoadScene("Stage2");
                break;
        }
    }
}
