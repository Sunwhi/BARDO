using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveSlotNamePanel : UIBase
{
    [SerializeField] Button YesBtn;
    [SerializeField] Button NoBtn;
    [SerializeField] TMP_InputField inputField;

    //param guide
    // param[0] : EYesNoPanelType (type of panel to open)
    // param[1] : UnityAction (action to perform on Yes button click, optional)
    // param[2] : UnityAction (action to perform on No button click, optional, defaults to NoAction if not provided)
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

        if (param.Length > 1 && param[1] is UnityAction<string> noAction)
        {
            NoBtn.onClick.AddListener(() => {
                noAction.Invoke(inputField.text);
                BaseAction();
            });
        }
        else
        {
            NoBtn.onClick.AddListener(BaseAction);
        }
    }

    private void BaseAction()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        UIManager.Hide();
    }
}
