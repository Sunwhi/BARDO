using UnityEngine;

public class ElevatorScene : UIBase
{
    [SerializeField] GameObject bg1;
    [SerializeField] GameObject bg2;

    private void OnEnable()
    {
        GameEventBus.Subscribe<ElevatorSeqEndEvent>(OnSequenceEnd);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ElevatorSeqEndEvent>(OnSequenceEnd);
    }
    private void Start()
    {
        bg1.SetActive(true);
        bg2.SetActive(true);
    }

    private void OnSequenceEnd(ElevatorSeqEndEvent evt)
    {
        UIManager.Hide(true);
        UIManager.Destroy(this.gameObject);
    }
}
