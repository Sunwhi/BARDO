using UnityEngine;  

public class Stage2CutScene : UIBase
{
    [SerializeField] private Animator cutsceneAnimator;
    private Closeup previousCloseup;

    private void Update()
    {
        Closeup currentCloseup = DialogueManager.Instance.closeup;

        if (Input.GetKeyDown(KeyCode.F)) CutsceneFinished();
        /*if (previousCloseup == Closeup.PadmaFly && currentCloseup == previousCloseup) 
        {
            CutsceneFinished();
            return; 
        }*/

        switch (currentCloseup)
        {
            case Closeup.None:
                break;
            case Closeup.Double:
                cutsceneAnimator.Play("CutDoubleAnim");
                break;
            case Closeup.Bardo:
                cutsceneAnimator.Play("CutBardoAnim");
                break;
            case Closeup.Padma:
                cutsceneAnimator.Play("CutPadmaAnim");
                break;
            case Closeup.PadmaFly:
                cutsceneAnimator.Play("CutPadmaFlyAnim");
                break;
        }
        previousCloseup = currentCloseup;
    }
    public void CutsceneFinished()
    {
        Debug.Log("finished");
        StoryManager.Instance.S2_CutsceneFin();
        Destroy(this.gameObject);
    }

}
