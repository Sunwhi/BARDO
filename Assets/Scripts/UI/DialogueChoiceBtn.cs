using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Composites;

public class DialogueChoiceBtn : MonoBehaviour, ISelectHandler
{

    [Header("Components")]
    [SerializeField] private Button choiceBtn;
    [SerializeField] private TextMeshProUGUI choiceText;

    private int choiceIndex = -1;
    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        choiceBtn.Select();
    }
    public void OnSelect(BaseEventData eventData)
    { 
        GameEventManager.Instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }

    public void OnSelectChoice()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Txt);
        GameEventManager.Instance.inputEvents.SubmitPressed();
    }
}
