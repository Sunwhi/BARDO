using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerGrounded : MonoBehaviour
{
    //[SerializeField] private Player player;

    //private float groundedTimer = 0f;
    //private float groundedGraceTime = 0.1f;
    //public bool isGrounded => groundedTimer > 0f;
    
    //private void Update()
    //{
    //    if (groundedTimer > 0f)
    //    {
    //        groundedTimer -= Time.deltaTime;
    //    }

    //    player.isGrounded = isGrounded;
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
    //        collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
    //    {
    //        foreach (var contact in collision.contacts)
    //        {
    //            if (contact.normal.y > 0.5f)
    //            {
    //                groundedTimer = groundedGraceTime;
    //                break;
    //            }
    //        }

            
    //    }
    //}
}

