using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * MySceneManager
 */
public enum ESceneType { // Scene의 이름들을 담는 enum 변수 -> LoadScene에 사용
    Title, Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7, MainScene
}
public class MySceneManager : Singleton<MySceneManager>
{
    // 씬을 로드함.
    public void LoadScene(ESceneType type)
    {
        switch(type)
        {
            case ESceneType.Title:
                SceneManager.LoadScene("TitleScene");
                break;
            case ESceneType.MainScene:
                SceneManager.LoadScene("MainScene");
                break;
        }
    }
}
