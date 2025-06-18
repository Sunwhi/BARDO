using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Stage1Panel : MonoBehaviour
{
    // Panel���� UIManager�� uiPanels���� �����ϱ� ���� Stage1�� �����ϴ� �гε��� ����Ʈ�� �����Ѵ�.
    [SerializeField] private List<GameObject> panelsToRegister;

    private void Start()
    {
        // panelsToRegister�� ���ִ� �гε��� UIManager�� uiPanels�� ����Ѵ�.
        foreach(var panel in panelsToRegister)
        {
            Debug.Log("ok");
            UIManager.Instance.RegisterPanels(panel);
        }
    }
}
