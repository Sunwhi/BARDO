using UnityEngine;

public class CardPanel : UIBase
{
    [SerializeField] private GameObject thread;
    [SerializeField] TaroCard[] cards;

    public override void Opened(object[] param)
    {
        thread.SetActive(false);
    }


}