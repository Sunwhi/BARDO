using System.Collections;
using UnityEngine;

public class RespawnManager : Singleton<RespawnManager>
{
    [SerializeField] private Player player;
    private readonly float delayBeforeRespawn = 0.5f;

    private void OnEnable()
    {
        GameEventBus.Subscribe<ViewportExitEvent>(OnViewportExit);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ViewportExitEvent>(OnViewportExit);
    }

    private void OnViewportExit(ViewportExitEvent e)
    {
        if (e.direction == ViewportExitDirection.Bottom || e.direction == ViewportExitDirection.Left)
        {
            StartCoroutine(HandleRespawn());
        }
    }

    private IEnumerator HandleRespawn()
    {
        //fade out
        yield return UIManager.Instance.fadeView.FadeOut();

        Vector3 respawnPos = TriggerManager.Instance.GetRespawnPosition();
        player.transform.position = respawnPos;

        ResetPlayer();

        //delay & fade in
        yield return new WaitForSeconds(delayBeforeRespawn);
        yield return UIManager.Instance.fadeView.FadeIn();
    }

    private void ResetPlayer()
    {
        player.spriteRenderer.flipX = false;
        player.rb.linearVelocity = Vector2.zero;
        player.fsm.ChangeState(player.fsm.IdleState);
    }
}