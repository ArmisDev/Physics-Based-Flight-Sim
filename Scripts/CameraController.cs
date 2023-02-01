using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("An array of transforms representing camera positions")]
    [SerializeField] Transform[] povs;
    [Tooltip("The speed at which the camera follows the plane")]
    [SerializeField] float _speed;

    private int _index = 0;
    private Vector3 target;

    private void Update()
    {
        //Allows you to change the FOV
        if (Input.GetKeyDown(KeyCode.Alpha1)) _index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) _index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) _index = 2;

        target = povs[_index].position;
    }

    private void FixedUpdate()
    {
        //Moves camera to desired position
        transform.position = Vector3.MoveTowards(transform.position, target,Time.deltaTime * _speed);
        transform.forward = povs[_index].forward;
    }
}
