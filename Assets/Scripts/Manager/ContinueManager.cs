using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ContinueManager : Singleton<ContinueManager>
{
    private Player player;
    private GameObject playerObj;
    public PlayerController controller;
    private Vector3 savedPosition;
    public bool loadedByContinue;

    private void OnEnable()
    {
        GameEventBus.Subscribe<ClickContinueEvent>(ContinueGame);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ClickContinueEvent>(ContinueGame);
    }

    private void ContinueGame(ClickContinueEvent ev)
    {
        SoundManager.Instance.PlayBGM(eBGM.Stage1);

        loadedByContinue = true;
        StartCoroutine(SetPlayerPositionDelayed());
        StartCoroutine(FadeIn());
    }

    private IEnumerator SetPlayerPositionDelayed()
    {
        yield return null;
        playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<Player>();

        savedPosition = SaveManager.Instance.MySaveData.savedPosition.ToVector3();
        controller = player.controller;
        player.GetComponent<PlayerInput>().enabled = true;
        player.rb.linearVelocity = Vector2.zero;

        playerObj.transform.position = savedPosition;
    }

    private IEnumerator FadeIn()
    {
        yield return null;
        yield return UIManager.Instance.fadeView.FadeIn(2f);
    }
}
