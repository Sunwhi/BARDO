using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EYesNoPanelType
{
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
        { EYesNoPanelType.Continue, ("Continue", "Do you want to continue?") },
        { EYesNoPanelType.Quit, ("Quit", "Are you sure you want to quit?") },
        { EYesNoPanelType.Save, ("Save", "Do you want to save your progress?") }
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
            YesBtn.onClick.AddListener(() => { yesAction.Invoke(); UIManager.Instance.HidePanel(name); });
        }

        if (param.Length > 2 && param[2] is UnityAction noAction)
        {
            NoBtn.onClick.AddListener(() => { noAction.Invoke(); UIManager.Instance.HidePanel(name); });
        }
        else
        {
            NoBtn.onClick.AddListener(NoAction);
        }
    }

    public void NoAction()
    {
        UIManager.Instance.HidePanel(name);
    }
}
