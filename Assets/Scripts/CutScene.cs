using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CutScene : UIBase
{
    [SerializeField] private Animator cutsceneAnimator;

    private void Update()
    {
        Closeup currentCloseup = DialogueManager.Instance.closeup;
        switch(currentCloseup)
        {
            case Closeup.None:
                break;
            case Closeup.Double:
                cutsceneAnimator.Play("CutDoubleAnim");
                Debug.Log("asdg");
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
    }


}
