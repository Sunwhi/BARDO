using DG.Tweening;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using System.ComponentModel.Design;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private Stage4Elevator elevator0;
    [SerializeField] private Stage4Elevator elevator1;

    private Vector3 elevator0Pos;
    private Vector3 elevator1Pos;
    private Vector3 middlePos = new Vector3(554.5999755859375f, -335.0f, 0f);

    public TaskCompletionSource<bool> tcs;

    private void Awake()
    {
        elevator0Pos = elevator0.transform.position;
        elevator1Pos = elevator1.transform.position;
    }

    public async void OnElevatorMove(bool isDown)
    {
        Stage4Elevator firstE = isDown ? elevator0 : elevator1;
        Stage4Elevator secondE = isDown ? elevator1 : elevator0;

        secondE.transform.position = middlePos;

        StoryManager.Instance.Player.isDownAllowed = true;
        //StartCoroutine(UIManager.Instance.fadeView.FadeOut(1f));
        //StartCoroutine(UIManager.Instance.fadeView.FadeIn(1f));
        await firstE.transform
            .DOMove(middlePos, 7f)
            .SetEase(Ease.InOutSine)
            .AsyncWaitForCompletion();


        if (SaveManager.Instance.MySaveData.stageIdx == 4)
        {
            if (tcs == null || tcs.Task.IsCompleted)
                tcs = new TaskCompletionSource<bool>();

            // 대화 시작
            // tcs를 사용하여 대화가 끝날 때까지
            // 추천 : tcs를 어떤식으로든 인자로 끌고 간 후, 대화 종료 시점에 
            // tcs.SetResult(true); 호출
            StartCoroutine(ElevatorSceneStart());

            await tcs.Task;
        }


        firstE.gameObject.SetActive(false);
        //StartCoroutine(UIManager.Instance.fadeView.FadeOut(1f));
        //StartCoroutine(UIManager.Instance.fadeView.FadeIn(1f));
        
        Vector3 targetSecond = (secondE == elevator0) ? elevator0Pos : elevator1Pos;
        await secondE.transform
            .DOMove(targetSecond, 7f)
            .SetEase(Ease.InOutSine)
            .AsyncWaitForCompletion();

        Vector3 targetFirst = (firstE == elevator0) ? elevator0Pos : elevator1Pos;
        firstE.gameObject.SetActive(true);
        firstE.transform.position = targetFirst;

        StoryManager.Instance.Player.isDownAllowed = false;
        StartCoroutine(secondE.DoorInteract(true, secondE == elevator0));
        
        // 엘리베이터 퀘스트에서만 subquest 클리어처리.
        if(SaveManager.Instance.MySaveData.currentQuest.QuestID == 3)
        {
            QuestManager.Instance.ClearSubQuest(0);
        }
    }

    private IEnumerator ElevatorSceneStart() 
    {
        UIManager.Show<ElevatorScene>();
        StoryManager.Instance.S4_ElevatorIn();
        yield return null;

        yield return new WaitUntil(() => !DialogueManager.Instance.dialoguePlaying);

        if(tcs != null && !tcs.Task.IsCompleted)
        {
            tcs.SetResult(true);
            GameEventBus.Raise(new ElevatorSeqEndEvent());
        }
    }
}