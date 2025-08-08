using UnityEngine;

/*
 * 이어하기를 했을 때 MainScene의 필요없는 부분들을 Inactive시킨다.
 * 안하면 Dialogue, 초반 걸어가는 부분, tutorial등이 모두 다시 나옴.
 */
public class InActiveObjects : MonoBehaviour
{
    public GameObject StoryManager;
    public GameObject FadeView;
    public GameObject Padma;
    public GameObject Tutorial_Triggers;
    public GameObject Tuto_Move_On;

    private void Start()
    {
        if(ContinueManager.Instance.loadedByContinue)
            InactiveObjects();
    }

    public void InactiveObjects()
    {
        Debug.Log("inactive");
        StoryManager.SetActive(false);
        FadeView.SetActive(false);
        Padma.SetActive(false);
        Tuto_Move_On.SetActive(true);
        //Tutorial_Triggers.SetActive(false);

        ContinueManager.Instance.loadedByContinue = false;
    }
}
