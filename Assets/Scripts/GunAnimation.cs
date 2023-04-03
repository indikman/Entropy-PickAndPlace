using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private Animator gunAnim;
    [SerializeField] private PlayerMovement playerMove;

    private int idleHash, moveHash, shootHash;


    void Start()
    {
        idleHash = Animator.StringToHash("idle");
        moveHash = Animator.StringToHash("moving");
        shootHash = Animator.StringToHash("shooting");
    }

    void Update()
    {

        if(playerMove.GetVelocity().magnitude > 0.1f)
        {
            gunAnim.SetBool(idleHash, false);
            gunAnim.SetBool(moveHash, true);
        }
        else
        {
            gunAnim.SetBool(idleHash, true);
            gunAnim.SetBool(moveHash, false);

        }

        gunAnim.SetBool(shootHash, Input.GetMouseButton(0));

        
    }
}
