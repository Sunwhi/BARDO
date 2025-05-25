using UnityEngine;

public class StageOptionUI : MonoBehaviour
{
    private void Start()
    {
        //GameObject optionPanel = this.gameObject;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ShowPanel("OptionPanel");
        }
    }
    
    public void OnClickContinue()
    {
        UIManager.Instance.HidePanel("OptionPanel");
    }

    public void OnClickTItle()
    {
        MySceneManager.Instance.LoadScene(SceneType.Title);
    }
}
