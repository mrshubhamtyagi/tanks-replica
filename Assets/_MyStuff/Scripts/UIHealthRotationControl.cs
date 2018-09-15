using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthRotationControl : MonoBehaviour
{
    public bool freezeRotation;
    private Quaternion originalRotation;

    private void OnEnable()
    {
        freezeRotation = true;
        originalRotation = transform.parent.localRotation; // canvas rotation
    }

    void Update()
    {
        // we dont want to change health slider/ canvas to chnage its rotation
        // as the tank rotates.
        // set rotation to original/default rotation in every frame.
        if (freezeRotation)
            transform.rotation = originalRotation;
    }
}
