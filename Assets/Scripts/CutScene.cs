using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Image cutsceneImage;
    private Sprite[] cutsceneDouble;
    private Sprite[] cutsceneBardo;
    private Sprite[] cutscenePadmaA;
    private Sprite[] cutscenePadmaB;
    [SerializeField] private float duration = 0.01f;
    bool keycode;
    private void Start()
    {
        cutsceneBardo = Resources.LoadAll<Sprite>("Sprites/CutScene/Stage2_Bardo");
        cutsceneDouble = Resources.LoadAll<Sprite>("Sprites/CutScene/Stage2_Double");
        cutscenePadmaA = Resources.LoadAll<Sprite>("Sprites/CutScene/Stage2_Padma_A");
        cutscenePadmaB = Resources.LoadAll<Sprite>("Sprites/CutScene/Stage2_Padma_B");
        StartCoroutine(PlayCutscene());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) keycode = true;
    }
    private IEnumerator PlayCutscene()
    {
        yield return PlayCutsceneDouble();
        yield return PlayCutsceneBardo();
        yield return PlayCutscenePadmaA();
        yield return PlayCutscenePadmaB();
    }
    private IEnumerator PlayCutsceneDouble()
    {
        while (true)
        {
            foreach (var sprite in cutsceneDouble)
            {
                cutsceneImage.sprite = sprite;
                yield return new WaitForSeconds(duration);
            }
            if (keycode)
            {
                keycode = false;
                break;
            }
        }
    }
    private IEnumerator PlayCutsceneBardo()
    {
        while (true)
        {
            foreach (var sprite in cutsceneBardo)
            {
                cutsceneImage.sprite = sprite;
                yield return new WaitForSeconds(duration);
            }
            if (keycode)
            {
                keycode = false;
                break;
            }
        }
    }
    private IEnumerator PlayCutscenePadmaA()
    {
        while (true)
        {
            foreach (var sprite in cutscenePadmaA)
            {
                cutsceneImage.sprite = sprite;
                yield return new WaitForSeconds(duration);
            }
            if (keycode)
            {
                keycode = false;
                break;
            }
        }
    }
    private IEnumerator PlayCutscenePadmaB()
    {
        while (true)
        {
            foreach (var sprite in cutscenePadmaB)
            {
                cutsceneImage.sprite = sprite;
                yield return new WaitForSeconds(duration);
            }
            if (keycode)
            {
                keycode = false;
                break;
            }
        }
    }
}
