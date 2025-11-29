using System.Collections;
using UnityEngine;

public class RoundTrigger4_2 : MonoBehaviour
{
    [SerializeField] private Transform stage4_3;

    //TODO : CollisionEnter 대신 대화의 마지막에서 자동진입하도록 설정
    //해당 코드는 StoryManager에서 만드는 것 추천.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.Instance.StopBGM();
        StartCoroutine(PlayBGMMaze());

        QuestManager.Instance.SetNewQuest();
        QuestManager.Instance.ShowQuestUI();

        Player p = StoryManager.Instance.Player;
        p.transform.position = stage4_3.position;
    }

    IEnumerator PlayBGMMaze()
    {
        yield return new WaitForSeconds(4f);
        SoundManager.Instance.PlayBGM(EBGM.Stage4Maze);
    }
}