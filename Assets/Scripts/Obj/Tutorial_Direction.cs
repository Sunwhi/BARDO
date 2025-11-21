using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial_Direction : MonoBehaviour
{
    [SerializeField] Image upImg;
    [SerializeField] Image downImg;
    [SerializeField] Image leftImg;
    [SerializeField] Image rightImg;

    private bool imgShown = false;
    private Color originalColor;

    private void Awake()
    {
        originalColor = Color.white;
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe<TutorialEvent>(Show);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<TutorialEvent>(Show);
    }

    private void Update()
    {
        if (imgShown)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                // 어두운 색으로 변경 (Color32 사용)
                leftImg.color = new Color32(200, 200, 200, 255);
            }
            else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                leftImg.color = originalColor;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                // 저장해둔 원래 색상으로 복원
                rightImg.color = new Color32(200, 200, 200, 255);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                rightImg.color = originalColor;
            }
        }
    }
    private void Show(TutorialEvent ev)
    {
        switch(ev.tutorialId)
        {
            case "Move_On":
                if (!SaveManager.Instance.MySaveData.tutoDirectionComplete)
                {
                    //SoundManager.Instance.PlayBGM(EBGM.Stage1);

                    upImg.DOFade(1f, 2f);
                    downImg.DOFade(1f, 2f);
                    leftImg.DOFade(1f, 2f);
                    rightImg.DOFade(1f, 2f);
                    imgShown = true;
                }
                break;
            case "Move_Off":
                QuestManager.Instance.ClearSubQuest(0);
                SaveManager.Instance.SetSaveData(nameof(SaveData.tutoDirectionComplete), true);
                upImg.DOFade(0f, 2f);
                downImg.DOFade(0f, 2f);
                leftImg.DOFade(0f, 2f);
                rightImg.DOFade(0f, 2f).OnComplete(() =>
                {
                    GameEventBus.Raise(new TutorialEvent("Jump_On"));
                });
                imgShown = false;
                break;  
        }
    }
}
