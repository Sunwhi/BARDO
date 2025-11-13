using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EYesNoPanelType
{
    New,
    Continue,
    Quit,
    Save,
}

public class YesNoPanel : UIBase
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI contentTxt;
    [SerializeField] private Button YesBtn;
    [SerializeField] private Button NoBtn;

    private readonly Dictionary< EYesNoPanelType, (string title, string content) > panelData = new()
    {
        {EYesNoPanelType.New, ("New Game", "저장 공간이 가득 찼습니다.\n가장 예전 기록이 삭제되고 새 게임이 시작됩니다.\n계속하시겠습니까?") },
        { EYesNoPanelType.Continue, ("Continue", "이어하시겠습니까?") },
        { EYesNoPanelType.Quit, ("Quit", "정말 나가시겠습니까? \n마지막 저장 내용 이후의 데이터는 삭제됩니다.") },
        { EYesNoPanelType.Save, ("Save", "이 슬롯을 덮어쓸까요?") }
    };

    //param guide
    // param[0] : EYesNoPanelType (type of panel to open)
    // param[1] : UnityAction (action to perform on Yes button click, optional)
    // param[2] : UnityAction (action to perform on No button click, optional, defaults to NoAction if not provided)
    public override void Opened(object[] param)
    {
        if (param.Length > 0 && param[0] is EYesNoPanelType panelType)
        {
            titleTxt.text = panelData[panelType].title;
            contentTxt.text = panelData[panelType].content;
        }

        YesBtn.onClick.RemoveAllListeners();
        NoBtn.onClick.RemoveAllListeners();

        if (param.Length > 1 && param[1] is UnityAction yesAction)
        {
            YesBtn.onClick.AddListener(() => {
                BaseAction();
                yesAction.Invoke();
            });
        }

        if (param.Length > 2 && param[2] is UnityAction noAction)
        {
            NoBtn.onClick.AddListener(() => {
                BaseAction();
                noAction.Invoke();
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
        UIManager.Hide(false);
    }
}
