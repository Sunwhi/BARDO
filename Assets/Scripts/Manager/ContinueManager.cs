using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinueManager : Singleton<ContinueManager>
{
    private Player player;
    private GameObject playerObj;
    public PlayerController controller;
    private Vector3 savedPosition;
    public bool loadedByContinue;

    [SerializeField]
    private Dictionary<int, CamState> curCamState = new()
    {
        {1, CamState.v1 },
        {2, CamState.v2_1 },
        {3, CamState.v3_1 },
        {4, CamState.v4_1 },
    };

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
        SoundManager.Instance.PlayBGM(EBGM.Stage1);

        loadedByContinue = true;
        StartCoroutine(SetPlayerPositionDelayed());
        StartCoroutine(SetCamera(SaveManager.Instance.MySaveData));
        StartCoroutine(FadeOut());
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
    private IEnumerator SetCamera(SaveData saveData)
    {
        yield return null;
        CameraManager.Instance.JumpAndCut(GetCameraState(saveData.stageIdx));
    }
    private CamState GetCameraState(int stageIdx)
    {
        return curCamState.TryGetValue(stageIdx, out var name) ? name : CamState.v1;
    }

    private IEnumerator FadeIn()
    {
        yield return null;
        yield return UIManager.Instance.fadeView.FadeIn(2f);
    }
    private IEnumerator FadeOut()
    {
        yield return null;
        yield return UIManager.Instance.fadeView.FadeOut(2f);
    }
}
