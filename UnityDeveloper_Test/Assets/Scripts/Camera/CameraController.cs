using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offsetPosition;
    private float  _rotY;
    [SerializeField]
    private float _rotSpeed = 3f;
    [SerializeField]
    private Space offsetPositionSpace = Space.Self;
    [SerializeField]
    private bool _invertX;
    private float _invertXValue;
    [SerializeField]
    private bool lookAt = true;

    private void Update()
    {
        Refresh();
    }

    // makes camera move as it is an child of player with mouse input rotation
    public void Refresh()
    {
        _invertXValue = (_invertX) ? -1 : 1;
        

        if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
          
            transform.position = target.TransformPoint(offsetPosition);         

        }
        else
        {
            transform.position = target.position + offsetPosition;
        }

        // compute rotation
        if (lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            _rotY += Input.GetAxis("Mouse X") * _invertXValue * _rotSpeed;
            var targetRotation = Quaternion.Euler(0, _rotY, 0);          
           // transform.rotation = targetRotation;

            transform.rotation = target.rotation*targetRotation;
        }
    }

   

}
