using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ARTopToPlace : MonoBehaviour
{
    [SerializeField] private AvatarImporter avatarImporter;
    [SerializeField] private Camera arCamera;

    private Vector3 newPosition = Vector3.zero;

    public Vector3 TouchPosition { get => newPosition; }

    public UnityEvent OnNewTouch;

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitAnything;

                if(Physics.Raycast(ray, out hitAnything, Mathf.Infinity))
                {
                    CreateOrMoveAvatar(hitAnything);
                }
            }
        }
    }

    private void CreateOrMoveAvatar(RaycastHit hit)
    {
        if (hit.transform.gameObject.CompareTag("Floor"))
        {
            if(avatarImporter.ImportedAvatar.activeSelf == false)
            {
                // spawn avatar
                avatarImporter.ImportedAvatar.SetActive(true);
                avatarImporter.ImportedAvatar.transform.rotation = hit.transform.rotation;
                avatarImporter.ImportedAvatar.transform.position = hit.transform.position;
            }
            else
            {
                // invoke event to move avatar
                newPosition = hit.point;
                OnNewTouch.Invoke();
            }
        }
    }
}
