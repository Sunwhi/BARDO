using UnityEngine;

public class Stage3_1Thread : MonoBehaviour
{
    Animator anim;

    public void OnThreadAnimEnd()
    {
        anim.SetBool("isMade", true);
    }
}
