using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * MySceneManager
 */
public enum SceneType { // Scene의 이름들을 담는 enum 변수 -> LoadScene에 사용
    Title, Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, MainScene
}
public class MySceneManager : Singleton<MySceneManager>
{
    // 씬을 로드함.
    public void LoadScene(SceneType type)
    {
        switch(type)
        {
            case SceneType.Title:
                SceneManager.LoadScene("TitleScene");
                break;
            case SceneType.MainScene:
                SceneManager.LoadScene("MainScene");
                break;
        }
    }
}
