using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private AvatarImporter avatarImporter;
    [SerializeField] private RuntimeAnimatorController avatarController;

    private Animator animator;

    private void OnEnable()
    {
        avatarImporter.OnAvatarStored.AddListener(AssignAvatarController);
    }

    private void AssignAvatarController()
    {
        animator = avatarImporter.ImportedAvatar.transform.GetComponent<Animator>();
        animator.runtimeAnimatorController = avatarController;
    }

    public void StartWalkAnimation()
    {
        this.animator.SetBool("isMoving", true);
    }

    public void StopWalkAnimation()
    {
        this.animator.SetBool("isMoving", false);
    }

    public void StartTurnAnimation(float angle)
    {
        if(angle > 0)
        {
            this.animator.SetBool("isTurnRight", true);
        }
        else
        {
            this.animator.SetBool("isTurnLeft", true);
        }
    }

    public void StopTurnAnimation()
    {
        this.animator.SetBool("isTurnleft", false);
        this.animator.SetBool("isTurnRight", false);
    }

    public bool IsMoveAnimatorPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Wolking");
    }

    public bool IsTurnAnimatorPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("RightTurn") || animator.GetCurrentAnimatorStateInfo(0).IsName("LeftTurn");
    }

    private void OnDisable()
    {
        avatarImporter.OnAvatarStored.RemoveListener(AssignAvatarController);
    }
}
