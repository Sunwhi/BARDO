using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private PlayableDirector menuDirector;
    [SerializeField] private PlayableDirector titleDirector;
    [SerializeField] private GameObject ContinueBtn;
    [SerializeField] private GameObject NewBtn;
    [SerializeField] private GameObject OptionBtn;
    [SerializeField] private GameObject CreditBtn;
    [SerializeField] private GameObject ExitBtn;
    [SerializeField] private GameObject IntroSkipBtn;

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
            SkipIntro();
        }
#endif
    }

    public void SkipIntro()
    {
        menuDirector?.Play();
        isMenuShown = true;
    }
    private void OnMenuDirectorFin(PlayableDirector director)
    {
        if(director == menuDirector)
        {
            ContinueBtn.SetActive(true);
            NewBtn.SetActive(true);
            OptionBtn.SetActive(true);
            CreditBtn.SetActive(true);
            ExitBtn.SetActive(true);
            IntroSkipBtn.SetActive(false);

            OnMenuDirectorFinEvent?.Invoke();
        }
    }
}
