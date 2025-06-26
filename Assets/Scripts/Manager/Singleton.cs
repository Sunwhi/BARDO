using Unity.VisualScripting;
using UnityEngine;
/*
 * Generic Singleton.
 * ��� �Ŵ���, �̱����� �� Singleton.cs�� ��ӽ�Ų��.
 */
public class Singleton<T> : MonoBehaviour where T : Component
{
    //private static bool isShuttingDown = false;

    private static T instance;
    public static T Instance
    {
        get 
        {
            //if (isShuttingDown) return null;

            if(instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));
                if(instance == null)
                {
                    Debug.Log("thisisget");
                    SetupInstance();
                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        RemoveDuplicates();
        Debug.Log("awake" + this.gameObject.name);
    }

    private static void SetupInstance()
    {
        instance = (T)FindAnyObjectByType(typeof(T));
        if(instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;
            instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }

    private void RemoveDuplicates()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        instance = null;
        if(instance == null)
        {
            Debug.Log("destroy" + this.gameObject.name);
        }
    }
}
