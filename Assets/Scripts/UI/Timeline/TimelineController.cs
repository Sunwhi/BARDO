using UnityEngine;
using UnityEngine.Playables;
using System;
public class TimelineController : MonoBehaviour
{
    [SerializeField] PlayableDirector menuDirector;
    [SerializeField] PlayableDirector titleDirector;

    public Action MenuShown;
    public double currentTime;
    private bool isMenuShown = false;

    private void Update()
    {
        currentTime = titleDirector.time;

        if(currentTime > 12.0 && !isMenuShown)
        {
            menuDirector.Play();

            isMenuShown = true;
        }
        if(!isMenuShown && Input.GetKeyDown(KeyCode.Space))
        {
            menuDirector.Play();

            isMenuShown = true;
        }
    }
}
