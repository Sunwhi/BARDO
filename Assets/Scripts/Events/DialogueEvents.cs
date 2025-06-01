using UnityEngine;
using System;
using Ink.Runtime;
using System.Collections.Generic;
public class DialogueEvents : MonoBehaviour 
{
    // string�� �޾Ƽ� void�� ��ȯ�ϴ� �Լ��� ���� �� �ִ� delegate Ÿ��
    // �̺�Ʈ ����
    public event Action<string> onEnterDialogue;

    public void EnterDialogue(string knotName)
    {
        //onEnterDialogue �̺�Ʈ�� ������� ������, ��ϵ� ��� �ݹ��� knotName ���ڷ� ȣ��
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
