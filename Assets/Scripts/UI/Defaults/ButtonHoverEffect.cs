using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler   
{
    [SerializeField] GameObject hoverStateObject;
    [SerializeField] TimelineController timelineController;

    private bool menuDirFin = false;
    private void OnEnable()
    {
        timelineController.OnMenuDirectorFinEvent += HandleMenuDirFin;
    }
    private void OnDisable()
    {
        timelineController.OnMenuDirectorFinEvent -= HandleMenuDirFin;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Hover);
        if(hoverStateObject != null && menuDirFin) hoverStateObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        if (hoverStateObject != null) hoverStateObject.SetActive(false);
    }

    private void HandleMenuDirFin()
    {
        menuDirFin = true;
    }
}
