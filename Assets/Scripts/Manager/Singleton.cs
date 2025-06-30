using Unity.VisualScripting;
using UnityEngine;
/*
 * Generic Singleton.
 * 모든 매니저, 싱글톤을 이 Singleton.cs에 상속시킨다.
 */
public class Singleton<T> : MonoBehaviour where T : Component
{
    //private static bool isShuttingDown = false;
    public static bool isManagerDestroyed = false; // 한번 singleton을 상속받은 Manager가 destroy되면 OnDisable()이든 어디든 접근 해서 다시 생성되지 않게.
    
    private static T instance;
    public static T Instance
    {
        get 
        {
            //if (isShuttingDown) return null;
            if (isManagerDestroyed) return null;

            if(instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));
                if(instance == null)
                {
                    //Debug.Log("thisisget");
                    SetupInstance();
                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        RemoveDuplicates();
        //Debug.Log("awake" + this.gameObject.name);
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
            //Debug.Log("destroy" + this.gameObject.name);
        }
        isManagerDestroyed = true;
    }
}
