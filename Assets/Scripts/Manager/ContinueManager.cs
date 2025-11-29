using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinueManager : Singleton<ContinueManager>
{
    private Player player;
    public PlayerController controller;
    private Vector3 savedPosition;
    public bool loadedByContinue;

    [SerializeField]
    private Dictionary<(int,int), CamState> curCamState = new()
    {
        {(1, 0), CamState.v1 }, {(2, 0), CamState.v2_0 }, {(2, 1), CamState.v2_1 }, {(2, 2), CamState.v2_1 }, {(2, 3), CamState.v2_3 }, {(2, 4), CamState.v2_3 }, {(2, 5), CamState.v2_2 },
        {(3, 0), CamState.v3_1 }, {(3,1), CamState.v3_1 }, {(3,2), CamState.v3_1 }, {(3,3), CamState.v3_1 },
        {(4, 0), CamState.v4_0 },
    };

    private Dictionary<int, EBGM> stageBGM = new()
    {
        {1,EBGM.Stage1 }, {2, EBGM.Stage2 }, {3, EBGM.Stage3 }, {4, EBGM.Stage4 },
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
        SoundManager.Instance.PlayBGM(stageBGM[SaveManager.Instance.MySaveData.stageIdx]);

        loadedByContinue = true;
        StartCoroutine(SetPlayerPositionDelayed());
        StartCoroutine(SetCamera(SaveManager.Instance.MySaveData));
        StartCoroutine(FadeOut());
        StartCoroutine(FadeIn());
        
    }
    private IEnumerator SetPlayerPositionDelayed()
    {
        yield return null;
        player = StoryManager.Instance.Player;

        savedPosition = SaveManager.Instance.MySaveData.savedPosition.ToVector3();
        controller = player.controller;
        player.playerInput.enabled = true;
        player.rb.linearVelocity = Vector2.zero;

        player.transform.position = savedPosition;
        player.controller.ResetInput();
    }
    private IEnumerator SetCamera(SaveData saveData)
    {
        yield return null;
        CameraManager.Instance.JumpAndCut(GetCameraState(saveData.stageIdx, saveData.storyIdx));
    }
    private CamState GetCameraState(int stageIdx, int storyIdx)
    {
        return curCamState.TryGetValue((stageIdx, storyIdx), out var name) ? name : CamState.v1;
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
