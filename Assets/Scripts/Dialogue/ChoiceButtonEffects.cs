using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Video;
using System;

public class ChoiceButtonEffects : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] List<Button> choiceButtons = new List<Button>();
    public string enterChoiceBtn;
    public  void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hover");

        switch (eventData.selectedObject.name)
        {
            case "DialogueChoiceBtn0":
                var selectable = choiceButtons[0] as Selectable;
                //choiceButtons[0].DoStateTransition(SelectionState.Selected);
                break;
            case "DialogueChoiceBtn1":
                break;
            case "DialogueChoiceBtn2":
                break;
            case "DialogueChoiceBtn3":
                break;
        }
        choiceButtons[0].colors = new ColorBlock();
    }
}
