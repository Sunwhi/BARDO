using UnityEngine;

public class Stage3_1Thread : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        if (SaveManager.Instance.MySaveData.quest1Completed)
        {
            anim.SetBool("isMade", false);
            anim.SetBool("isMade", true);
        }
    }

    public void OnThreadAnimEnd()
    {
        anim.SetBool("isMade", true);
        SaveManager.Instance.SetSaveData(nameof(SaveData.quest1Completed), true);
    }
}
