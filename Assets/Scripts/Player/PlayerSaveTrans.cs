using UnityEngine;

public class PlayerSaveTrans : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Vector3 playerPosition;

    private void OnEnable()
    {
        GameEventBus.Subscribe<TransitionEvents>(OnTransitionEvent);
        playerPosition = this.transform.position;
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<TransitionEvents>(OnTransitionEvent);
    }

    private void OnTransitionEvent(TransitionEvents ev)
    {
        if(id == ev.stageId)    SaveManager.Instance.SavePlayerPosition(id, playerPosition);
    }
}
