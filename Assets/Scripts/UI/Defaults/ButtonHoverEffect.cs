using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler   
{
    [SerializeField] GameObject hoverStateObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("aoieisojf");
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Hover);
        if(hoverStateObject != null) hoverStateObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        if (hoverStateObject != null) hoverStateObject.SetActive(false);
    }
}
