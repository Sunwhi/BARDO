using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Composites;
using Unity.VisualScripting;
using System;
using UnityEditor.Rendering;

public class DialogueChoiceBtn : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Components")]
    [SerializeField] private Button choiceBtn;
    [SerializeField] private TextMeshProUGUI choiceText;

    public static event Action onEnterChoiceBtn;
    public static event Action onExitChoiceBtn;

    private static string selectedChoiceBtn;
    private static string enteredChoiceBtn;
    private static string exitedChoiceBtn;
    ColorBlock cb;
    private void Start()
    {
        cb = choiceBtn.colors;
    }

    private int choiceIndex = -1;

    private void Update()
    {
        //Debug.Log("selected : " + selectedChoiceBtn);
        //Debug.Log("entered : " + enteredChoiceBtn);
    }
    private void OnEnable()
    {
        onEnterChoiceBtn += EnterChoiceBtn;
        onExitChoiceBtn += ExitChoiceBtn;
    }
    private void OnDisable()
    {
        onEnterChoiceBtn -= EnterChoiceBtn;
        onExitChoiceBtn -= ExitChoiceBtn;
    }
    private void EnterChoiceBtn()
    {
        // entered �����ϰ� ��� ���������� selectedColor = white
        if (gameObject.name != enteredChoiceBtn)
        {
            Debug.Log("enter : " + gameObject.name);
            cb.selectedColor = Color.white;
        }
        choiceBtn.colors = cb;
        //selectedChoiceBtn�� ���� �ٲٸ� ��.
    }
    private void ExitChoiceBtn()
    {
        // exited �����ϰ� ��� ���������� selectedColor = gray
        if(gameObject.name != exitedChoiceBtn)
        {
            Debug.Log("exit : " + gameObject.name);
            cb.selectedColor = new Color32(180, 180, 180, 255);
        }
        choiceBtn.colors = cb;
    }
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
        //Debug.Log(gameObject.name);
        selectedChoiceBtn = gameObject.name;
        GameEventManager.Instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }

    public void OnSelectChoice()
    {
        // ���� ���������� ������ �� ����
        if (DialogueManager.Instance.canContinueToNextLine)
        {
            SoundManager.Instance.PlaySFX(eSFX.UI_Button_Txt);
            GameEventManager.Instance.inputEvents.StartDialogue();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name);
        enteredChoiceBtn = gameObject.name;
        SelectButton(); // ���콺�� Ŭ���� ��ư���� Select ����
        onEnterChoiceBtn?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        exitedChoiceBtn = gameObject.name;
        onExitChoiceBtn?.Invoke();
    }
}
