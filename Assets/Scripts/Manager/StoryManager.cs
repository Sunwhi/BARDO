using System.Collections;
using UnityEngine;

public class StoryManager : Singleton<StoryManager>
{
    [SerializeField] private Player player;

    private IEnumerator Start()
    {
        SoundManager.Instance.PlaySFX(eSFX.Stage_Transition);
        yield return new WaitForSeconds(1f);

        SoundManager.Instance.PlaySFX(eSFX.Opening_Door);
        yield return UIManager.Instance.fadeView.FadeIn();

        SoundManager.Instance.PlayBGM(eBGM.Stage1);
        SoundManager.Instance.PlayAmbientSound(eSFX.Background_Wind);

        yield return PlayerWalkOut();
    }

    private IEnumerator PlayerWalkOut()
    {
        player.ForceMove(new Vector2(1, 0));
        yield return new WaitForSeconds(1f);
        player.ForceMove(Vector2.zero);
    }
}
