using System;
using UnityEngine;
using UnityEngine.Animations;

public class RotateConstantSmooth : MonoBehaviour
{
    public Axis RotationAxis;
    public float RotationSpeed;


    private void Update()
    {
        Vector3 rotationAxis = Vector3.zero;
        switch (RotationAxis)
        {
            case Axis.X:
                rotationAxis = new Vector3(1, 0, 0);
                break;
            case Axis.Y:
                rotationAxis = new Vector3(0, 1, 0);
                break;
            case Axis.Z:
                rotationAxis = new Vector3(0, 0, 1);
                break;
        }
        this.transform.Rotate(rotationAxis, RotationSpeed * Time.deltaTime);
    }
}
