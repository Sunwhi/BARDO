using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CutScene : UIBase
{
    [SerializeField] private Animator cutsceneAnimator;
    private Closeup previousCloseup;

    private void Update()
    {
        Closeup currentCloseup = DialogueManager.Instance.closeup;

        if (currentCloseup == previousCloseup) 
        {
            return; 
        }

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
        Debug.Log("finish");
        StoryManager.Instance.Player.playerInput.enabled = true;
        Destroy(this.gameObject);
    }

}
