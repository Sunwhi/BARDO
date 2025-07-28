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
        player.controller.InputEnabled = false;

        if (e.direction == ViewportExitDirection.Bottom)
        {
            StartCoroutine(HandleRespawn());
        }
        else if (e.direction == ViewportExitDirection.Left)
        {
            Debug.LogWarning("왼쪽에서 벗어났습니다. 리스폰 처리하지 않습니다.");
        }
    }

    private IEnumerator HandleRespawn()
    {
        //fade out
        yield return UIManager.Instance.fadeView.FadeOut();

        ResetPlayer();

        //delay & fade in
        yield return new WaitForSeconds(delayBeforeRespawn);
        yield return UIManager.Instance.fadeView.FadeIn();
        
        //Player 입력 활성화
        player.controller.InputEnabled = true;
    }

    private void ResetPlayer()
    {
        player.controller.ResetInput();
        player.rb.linearVelocity = Vector2.zero;
        player.fsm.ChangeState(player.fsm.IdleState);

        Vector3 respawnPos = TriggerManager.Instance.GetRespawnPosition();
        player.transform.position = respawnPos;
        if (player.transform.localScale.x < 0)
        {
            Vector3 scale = player.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            player.transform.localScale = scale;
        }
    }
}