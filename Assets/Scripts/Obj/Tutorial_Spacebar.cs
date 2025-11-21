using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Spacebar : MonoBehaviour
{
    [SerializeField] Image spaceImg;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 어두운 색으로 변경 (Color32 사용)
                spaceImg.color = new Color32(200, 200, 200, 255);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                // 저장해둔 원래 색상으로 복원
                spaceImg.color = originalColor;
            }
        }
    }
    private void Show(TutorialEvent ev)
    {
        switch (ev.tutorialId)
        {
            case "Jump_On":
                if (!SaveManager.Instance.MySaveData.tutoJumpComplete)
                {
                    SoundManager.Instance.PlayBGM(EBGM.Stage1);

                    spaceImg.DOFade(1f, 2f);
                    imgShown = true;
                }
                break;
            case "Jump_Off":
                QuestManager.Instance.ClearSubQuest(1);
                SaveManager.Instance.SetSaveData(nameof(SaveData.tutoJumpComplete), true);
                spaceImg.DOFade(0f, 2f);
                imgShown= false;
                break;
        }
    }
}
