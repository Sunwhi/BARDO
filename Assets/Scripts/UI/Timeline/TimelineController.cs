using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private PlayableDirector menuDirector;
    [SerializeField] private PlayableDirector titleDirector;

    public event Action OnMenuDirectorFinEvent;
    public double currentTime;
    private bool isMenuShown = false;
    

    private void OnEnable()
    {
        if(menuDirector != null)
        {
            menuDirector.stopped += OnMenuDirectorFin;
        }
    }
    private void OnDisable()
    {
        if (menuDirector != null)
        {
            menuDirector.stopped -= OnMenuDirectorFin;
        }
    }

    private void Update()
    {
        if (titleDirector != null)  currentTime = titleDirector.time;

        if (currentTime > 12.0 && !isMenuShown)
        {
            menuDirector?.Play();
            isMenuShown = true;
        }
#if UNITY_EDITOR
        // 스페이스 누르면 스킵
        if (!isMenuShown && Input.GetKeyDown(KeyCode.Space))
        {
            menuDirector?.Play();
            isMenuShown = true;
        }
#endif
    }

    private void OnMenuDirectorFin(PlayableDirector director)
    {
        if(director == menuDirector)
        {
            OnMenuDirectorFinEvent?.Invoke();
        }
    }
}
