using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneDetection : MonoBehaviour
{
    [SerializeField] private ARPlaneManager aRPlaneManager;

    private bool isFloorPlaced;

    private List<ARPlane> foundPlanes = new List<ARPlane>();
    private ARPlane foundPlane;

    private void OnEnable()
    {
        aRPlaneManager.planesChanged += PlanesChanged;
    }

    private void OnDesable()
    {
        aRPlaneManager.planesChanged -= PlanesChanged;
    }

    private void PlanesChanged(ARPlanesChangedEventArgs obj)
    {

        if (obj != null && obj.added.Count > 0)
            foundPlanes.AddRange(obj.added);

        foreach(ARPlane plane in foundPlanes)
        {
            if(plane.extents.x * plane.extents.y >= 0.5f && !isFloorPlaced)
            {
                isFloorPlaced = true;
                foundPlane = plane;
                foundPlane.tag = "Floor";
                DisablePlanes();
            }
        }
    }

    private void DisablePlanes()
    {
        aRPlaneManager.enabled = false;

        foreach(var plane in aRPlaneManager.trackables)
        {
            if (plane != foundPlane)
                plane.gameObject.SetActive(false);
        }

        this.enabled = false;
    }

}