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
        { eItemPanelType.Karmic_Shard, ("업경대", "업경대는 영혼의 힘을 모아 업경을 강화하는 데 사용됩니다.") },
        { eItemPanelType.Memory_Lamp, ("등명석", "등명석은 어둠 속에서 길을 밝혀주는 빛나는 돌입니다.") },
        { eItemPanelType.Soul_Thread, ("혼의 실", "혼의실은 영혼이 모이는 장소로, 특별한 힘을 지니고 있습니다.") }
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
}
