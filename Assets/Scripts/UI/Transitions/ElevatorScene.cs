using UnityEngine;

public class ElevatorScene : UIBase
{
    [SerializeField] GameObject bg1;
    [SerializeField] GameObject bg2;

    private void Start()
    {
        bg1.SetActive(true);
        bg2.SetActive(true);
    }
}
