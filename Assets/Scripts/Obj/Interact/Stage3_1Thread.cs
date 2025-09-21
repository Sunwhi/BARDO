using UnityEngine;

public class Stage3_1Thread : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void Start()
    {
        if (SaveManager.Instance.MySaveData.quest1Completed)
        {
            anim.SetBool("isMade", true);
        }
    }

    public void OnThreadAnimEnd()
    {
        anim.SetBool("isMade", true);
        SaveManager.Instance.SetSaveData(nameof(SaveData.quest1Completed), true);
    }
}
