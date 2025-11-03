using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveSlotNamePanel : UIBase
{
    [SerializeField] Button YesBtn;
    [SerializeField] Button NoBtn;
    public TMP_InputField inputField;

    //param guide
    // param[0] : UnityAction (action to perform on Yes button click, optional)
    // param[1] : UnityAction (action to perform on No button click, optional, defaults to NoAction if not provided)
    public override void Opened(object[] param)
    {
        YesBtn.onClick.RemoveAllListeners();
        NoBtn.onClick.RemoveAllListeners();

        if (param.Length > 0 && param[0] is UnityAction<string> yesAction)
        {
            YesBtn.onClick.AddListener(() => {
                yesAction.Invoke(inputField.text);
                BaseAction();
            });
        }

        if (param.Length > 1 && param[1] is int slotIdx)
        {
            inputField.text = SaveManager.Instance.SaveSlots[slotIdx].saveName; //기존 슬롯의 이름을 기본 inputField text로 설정
        }

        NoBtn.onClick.AddListener(BaseAction);
    }

    private void BaseAction()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Hide(false);
    }

    public override void OnUICloseBtn()
    {
        base.OnUICloseBtn();
    }
}
