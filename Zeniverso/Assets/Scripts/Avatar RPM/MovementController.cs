using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private AvatarImporter avatarImporter;
    [SerializeField] private ARTopToPlace aRTopToPlace;
    [SerializeField] private AvatarAnimationController avatarAnimationController;

    private Vector3 touchPos;
    private Transform avatarTransform = null;

    private bool isMoving;
    private bool isTurning;

    private void OnEnable()
    {
        aRTopToPlace.OnNewTouch.AddListener(OnTouch);
        avatarImporter.OnAvatarStored.AddListener(GetAvatarTransform);
    }

    private void OnDisable()
    {
        aRTopToPlace.OnNewTouch.RemoveListener(OnTouch);
        avatarImporter.OnAvatarStored.RemoveListener(GetAvatarTransform);
    }

    private void GetAvatarTransform()
    {
        avatarTransform = avatarImporter.ImportedAvatar.transform;
    }

    private void OnTouch()
    {
        avatarAnimationController.StopTurnAnimation();
        avatarAnimationController.StopWalkAnimation();

        touchPos = aRTopToPlace.TouchPosition;

        float angle = CalculateAngle(touchPos, avatarTransform.position, avatarTransform);

        if(angle == 0)
        {
            isTurning = false;
            isMoving = true;
        }
        else
        {
            isTurning = true;
            isMoving = false;

            avatarAnimationController.StartTurnAnimation(angle);
        }
    }

    private void Update()
    {
        if (isTurning)
        {
            if (Vector3.Dot(avatarTransform.forward, Vector3.Normalize(touchPos - avatarTransform.position)) > 0.9f)
            {
                avatarAnimationController.StopTurnAnimation();

                isMoving = true;
                isTurning = false;
            }
        }

        if (isMoving && !avatarAnimationController.IsTurnAnimatorPlaying())
        {
            if(Vector3.Distance(avatarTransform.position, touchPos) > 0.1f)
            {
                // nós queremos andar
                avatarTransform.LookAt(touchPos);

                if (!avatarAnimationController.IsTurnAnimatorPlaying())
                    avatarAnimationController.StartWalkAnimation();

                avatarTransform.position = Vector3.MoveTowards(avatarTransform.position, touchPos, Time.deltaTime * 0.3f);
            }
            else
            {
                // nós queremos parar de andar
                avatarAnimationController.StopWalkAnimation();
                isMoving = false;

            }
        }
    }

    private float CalculateAngle(Vector3 targetPos, Vector3 currentPos, Transform avatarTransform)
    {
        return Vector3.SignedAngle(currentPos - targetPos, avatarTransform.forward, avatarTransform.up);
    }
}