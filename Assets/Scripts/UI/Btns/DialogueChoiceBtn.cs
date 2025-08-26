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
        //SoundManager.Instance.PlaySFX(eSFX.UI_Button_Hover);
        // entered 제외하고 모든 선택지들의 selectedColor = white
        if (gameObject.name != enteredChoiceBtn)
        {
            //Debug.Log("enter : " + gameObject.name);
            cb.selectedColor = Color.white;
        }
        choiceBtn.colors = cb;
        //selectedChoiceBtn의 색을 바꾸면 됨.
    }
    private void ExitChoiceBtn()
    {
        // exited 제외하고 모든 선택지들의 selectedColor = gray
        if(gameObject.name != exitedChoiceBtn)
        {
            //Debug.Log("exit : " + gameObject.name);
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
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Hover);

        //Debug.Log(gameObject.name);
        selectedChoiceBtn = gameObject.name;
        DialogueEventManager.Instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }

    public void OnSelectChoice()
    {
        // 문장 끝날때까지 선택지 못 누름
        if (DialogueManager.Instance.canContinueToNextLine)
        {
            SoundManager.Instance.PlaySFX(ESFX.UI_Button_Txt);
            DialogueEventManager.Instance.inputEvents.StartDialogue();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //SoundManager.Instance.PlaySFX(eSFX.UI_Button_Hover);
        //Debug.Log(gameObject.name);
        enteredChoiceBtn = gameObject.name;
        SelectButton(); // 마우스로 클릭한 버튼으로 Select 변경
        onEnterChoiceBtn?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        exitedChoiceBtn = gameObject.name;
        onExitChoiceBtn?.Invoke();
    }
}
