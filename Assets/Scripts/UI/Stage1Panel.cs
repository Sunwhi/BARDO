using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Stage1Panel : MonoBehaviour
{
    // Panel들을 UIManager의 uiPanels에서 관리하기 위해 Stage1에 존재하는 패널들을 리스트에 저장한다.
    [SerializeField] private List<GameObject> panelsToRegister;

    private void Start()
    {
        // panelsToRegister에 들어가있는 패널들을 UIManager의 uiPanels에 등록한다.
        foreach(var panel in panelsToRegister)
        {
            Debug.Log("ok");
            UIManager.Instance.RegisterPanels(panel);
        }
    }
}
