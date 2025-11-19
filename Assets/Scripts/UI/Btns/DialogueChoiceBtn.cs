using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class DialogueChoiceBtn : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Components")]
    [SerializeField] private Button choiceBtn;
    [SerializeField] private TextMeshProUGUI choiceText;
    [SerializeField] private RectTransform choiceRectTransform;

    public static event Action onEnterChoiceBtn;
    public static event Action onExitChoiceBtn;
    public float textLength;
    
    private static string selectedChoiceBtn;
    private static string enteredChoiceBtn;
    private static string exitedChoiceBtn;
    private float choiceWidth;

    ColorBlock cb;

    private void Start()
    {
        cb = choiceBtn.colors;
    }

    private void Update()
    {

    }

    private int choiceIndex = -1;

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
        // entered 제외하고 모든 선택지들의 selectedColor = white
        if (gameObject.name != enteredChoiceBtn)
        {
            cb.selectedColor = Color.white;
        }
        choiceBtn.colors = cb;
    }
    private void ExitChoiceBtn()
    {
        // exited 제외하고 모든 선택지들의 selectedColor = gray
        if(gameObject.name != exitedChoiceBtn)
        {
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
        enteredChoiceBtn = gameObject.name;
        SelectButton(); // 마우스로 클릭한 버튼으로 Select 변경
        onEnterChoiceBtn?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        exitedChoiceBtn = gameObject.name;
        onExitChoiceBtn?.Invoke();
    }
    public float GetPrefferedWidth()
    {
        choiceText.ForceMeshUpdate();
        float textPrefferedWidth = choiceText.preferredWidth;
        Debug.Log(textPrefferedWidth);

        float calculatedWidth = textPrefferedWidth + 15 * Mathf.Log(textPrefferedWidth / 10, 2);//1 * (textPrefferedWidth / 4);//textPrefferedWidth + (0.5f) * (textPrefferedWidth - 90);
        Debug.Log("calculateed" + calculatedWidth);
        return calculatedWidth;
    }
    public void ControlChoiceWidth(float newWidth)
    {
        choiceRectTransform.sizeDelta = new Vector2(newWidth, choiceRectTransform.sizeDelta.y);
    }
}
