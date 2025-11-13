using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler   
{
    [SerializeField] GameObject hoverStateObject;
    [SerializeField] TimelineController timelineController;

    [SerializeField] Image exitBtn;
    [SerializeField] Sprite defaultExitSprite;
    [SerializeField] Sprite glowExitSprite;

    private bool menuDirFin = false;
    private void OnEnable()
    {
        if(timelineController != null)
        {
            timelineController.OnMenuDirectorFinEvent += HandleMenuDirFin;
        }
    }
    private void OnDisable()
    {
        if(timelineController != null)
        {
            timelineController.OnMenuDirectorFinEvent -= HandleMenuDirFin;
        }
        if(exitBtn != null)
        {
            exitBtn.sprite = defaultExitSprite;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Hover);

        if (hoverStateObject != null && menuDirFin)
        {
            hoverStateObject.SetActive(true); 
        }
        
        if(glowExitSprite != null)
        {
            exitBtn.sprite = glowExitSprite;
        }
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        if (hoverStateObject != null) hoverStateObject.SetActive(false);

        if (defaultExitSprite != null)
        {
            exitBtn.sprite = defaultExitSprite;
        }
    }

    private void HandleMenuDirFin()
    {
        menuDirFin = true;
    }
}
