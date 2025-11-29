using UnityEngine;

public class CardPanel : UIBase
{
    [SerializeField] TaroCard[] cards;

    private Vector3 stage4PlayerPos;
    private float BGMVolume;

    public override void Opened(object[] param)
    {
        //BGMVolume =  SoundManager.Instance.GetBGMVolume();
       // SoundManager.Instance.SetBGMVolume(BGMVolume - 0.2f);
        stage4PlayerPos = param.Length > 0 && param[0] is Vector3 pos ? pos : Vector3.zero;

        foreach (var card in cards)
        {
            card.NoticeNextPlayerPos(stage4PlayerPos);
        }
    }
    public override void Closed(object[] param)
    {
        //SoundManager.Instance.SetBGMVolume(BGMVolume);
        base.Closed(param);
    }
}