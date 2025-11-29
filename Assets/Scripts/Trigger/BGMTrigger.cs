using UnityEngine;

public class BGMTrigger : TriggerBase
{
    [SerializeField] EBGM bgm;

    protected override void OnTriggered()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }
}
