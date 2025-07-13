using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceButtonEffects : Button
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        DoStateTransition(SelectionState.Highlighted, false);
        //this.colors
    }

}
