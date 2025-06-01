using UnityEngine;
using System;
using Ink.Runtime;
using System.Collections.Generic;
public class DialogueEvents : MonoBehaviour 
{
    // string을 받아서 void를 반환하는 함수를 담을 수 있는 delegate 타입
    // 이벤트 정의
    public event Action<string> onEnterDialogue;

    public void EnterDialogue(string knotName)
    {
        //onEnterDialogue 이벤트가 비어있지 않으면, 등록된 모든 콜백을 knotName 인자로 호출
        onEnterDialogue?.Invoke(knotName);
    }


    public event Action onDialogueStarted;
    public void DialogueStarted()
    {
        onDialogueStarted?.Invoke();
    }


    public event Action onDialogueFinished;
    public void DialogueFinished()
    {
        onDialogueFinished?.Invoke();
    }

    public event Action<string, List<Choice>> onDisplayDialogue;
    public void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        onDisplayDialogue?.Invoke(dialogueLine, dialogueChoices);
    }


    public event Action<int> onUpdateChoiceIndex;

    public void UpdateChoiceIndex(int choiceIndex)
    {
        onUpdateChoiceIndex?.Invoke(choiceIndex);
    }
}
