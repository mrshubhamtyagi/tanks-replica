using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dampValue = 0.2f; // time to move & zoom the camera to desired position (for smoothness in smoothdamp)
    public float minOrthographicSize = 4f; // Max Zoom in Distance
    public float screenEdgeBuffer = 4f; // amount added to the camera size for edge offset
    [HideInInspector] public Transform[] targets;

    private new Camera camera;
    private float refZoomSpeed; // required a ref in smooth damp
    private Vector3 refMoveVelocity; // required a ref in smooth damp
    private Vector3 desiredPosition; // based on CameraController position and not on actual camera position

    void Start()
    {
        camera = GetComponentInChildren<Camera>();
    }

    void FixedUpdate()
    {
        // Moving and Zooming in FixedUpdate because of our 
        // tanks movements are also calculated in FixedUpdate
        // it will keep the sync between our tanks and camera
        Move();
        Zoom();
    }

    private void Move()
    {
        // Finding a center point of all active tanks in the scene
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref refMoveVelocity, dampValue);
    }

    private void FindAveragePosition()
    {
        Vector3 _avgPos = Vector3.zero;
        int _numberOfTanksFound = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!(targets[i].gameObject.activeSelf)) // skipping any inactive tank
                continue;

            _avgPos += targets[i].position;
            _numberOfTanksFound++;
        }

        if (_numberOfTanksFound > 0)
            _avgPos /= _numberOfTanksFound;

        _avgPos.y = transform.position.y; // dont want to change the y value

        desiredPosition = _avgPos;
    }

    private void Zoom()
    {
        // end point distance in y-axis from the center is orthographic size of the camea;
        // end point distance in x-axis from the center is orthographic size * aspect;
        // aspect = width /  height (eg: for resolution 1920:1080 ---> aspect will be 1.77);

        // finding orthographic size by the formula size = distance / aspect
        // here distance is the max distance of a tank from the center in either axis
        float requiredSize = FindRequiredOrthographicSize();

        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, requiredSize, ref refZoomSpeed, dampValue);
    }

    private float FindRequiredOrthographicSize()
    {
        float _requiredSize = 0f;

        // we need to calculate zoom size based on the desired position(camera controller position) and not on camera's position.
        // we need to convert desired position from world space to local space.
        // this conversion is required to get the local space as 
        // world space starts from bottom right and local space starts from center
        // and the formala to calculate the size requires distance from the center
        // we also need to do this conversion for each tank(target).
        Vector3 _desiredPosInLocalSpace = transform.InverseTransformPoint(desiredPosition);

        for (int i = 0; i < targets.Length; i++)
        {
            if (!(targets[i].gameObject.activeSelf))
                continue;

            Vector3 _targetPosInLocalSpace = transform.InverseTransformPoint(targets[i].position);

            Vector3 _distanceToTank = _targetPosInLocalSpace - _desiredPosInLocalSpace;

            // we need to find the max size between x and y
            _requiredSize = Mathf.Max(_requiredSize, Mathf.Abs(_distanceToTank.y));

            // for x-axis we need to / it by the aspect to find the size acc. to the formula
            _requiredSize = Mathf.Max(_requiredSize, Mathf.Abs(_distanceToTank.x) / camera.aspect);
        }

        _requiredSize += screenEdgeBuffer; // to maintain the edge offset

        // if the max of x and y is still less than minOrthographicSize
        // then set size to minOrthographicSize.
        if (_requiredSize < minOrthographicSize)
            _requiredSize = minOrthographicSize;

        return _requiredSize;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();
        transform.position = desiredPosition;

        camera.orthographicSize = FindRequiredOrthographicSize();
    }
}
