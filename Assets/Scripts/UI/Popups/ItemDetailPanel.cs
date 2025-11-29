using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum eItemPanelType
{
    Default,
    Karmic_Shard, //업경대
    Memory_Lamp, //등명석
    Soul_Thread, //혼의실
}

public class ItemDetailPanel : UIBase
{
    [SerializeField] private Image itemIconImg;
    [SerializeField] private Sprite[] itemIcons;

    [SerializeField] private TextMeshProUGUI itemTitleTxt;
    [SerializeField] private TextMeshProUGUI itemDescTxt;

    private readonly Dictionary<eItemPanelType, (string title, string desc)> itemData = new()
    {
        { eItemPanelType.Karmic_Shard, ("진실의 거울", "진실을 담아 보여줍니다.\n그런데 그 진실은 당신이 원하는 형태가\n아닐 수 있습니다.") },
        { eItemPanelType.Memory_Lamp, ("기억의 등불", "잃어버린 기억에 불을 붙여줍니다.\n하지만 뜨거워 다칠 수 있습니다.") },
        { eItemPanelType.Soul_Thread, ("혼의 실", "당신의 운명으로 이끌어줍니다.\n엉켜있다면 풀어야합니다.") }
    };
    private Transform stage3PlayerPos;

    public override void Opened(object[] param)
    {
        Time.timeScale = 0f;
        eItemPanelType itemType = param.Length > 0 && param[0] is eItemPanelType type ? type : eItemPanelType.Default;
        itemIconImg.sprite = itemIcons[(int)itemType];

        if (eItemPanelType.Default == itemType)
        {
            itemTitleTxt.text = "아이템 없음";
            itemDescTxt.text = "선택된 아이템이 없습니다.";
            return;
        }

        stage3PlayerPos = param.Length > 1 && param[1] is Transform pos ? pos : null;

        itemTitleTxt.text = itemData[itemType].title;
        itemDescTxt.text = itemData[itemType].desc;
    }

    public override void Closed(object[] param)
    {
        Time.timeScale = 1f;

        bool isAllItemsAcquired = true;
        foreach (var item in SaveManager.Instance.MySaveData.quest1ItemAcquired)
        {
            if (!item)
            {
                isAllItemsAcquired = false;
                break;
            }
        }

        if (isAllItemsAcquired && stage3PlayerPos != null)
        {
            GameEventBus.Raise<NextStageEvent>(new(3, stage3PlayerPos.position));
        }
    }

    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Mouse_Click);
        base.OnUICloseBtn();
    }
}
