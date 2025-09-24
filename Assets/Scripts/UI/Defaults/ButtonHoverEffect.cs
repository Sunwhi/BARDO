using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler   
{
    [SerializeField] GameObject normalStateObject;
    [SerializeField] GameObject hoverStateObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Hover);
        if(normalStateObject != null) normalStateObject.SetActive(false);
        if(hoverStateObject != null) hoverStateObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        if (normalStateObject != null) normalStateObject.SetActive(true);
        if (hoverStateObject != null) hoverStateObject.SetActive(false);
    }
}
