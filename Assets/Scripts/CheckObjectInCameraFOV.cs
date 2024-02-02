using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CheckObjectInCameraFOV : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject objectToCheck;
    public VoidEventChannel outOfCam;
    void Update()
    {
        if (mainCamera == null || objectToCheck == null)
            return;

        // Get the camera frustum planes
        Plane[] cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Check if the object's bounds are within the camera frustum
        if (GeometryUtility.TestPlanesAABB(cameraPlanes, objectToCheck.GetComponent<Renderer>().bounds))
        {
            Debug.Log("Object is in camera FOV.");
        }
        else
        {
            Debug.Log("Object is outside camera FOV.");
            outOfCam.raiseEvent();
        }
    }
}
