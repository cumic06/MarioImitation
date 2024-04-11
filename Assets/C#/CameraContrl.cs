using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContrl : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, 0, -2);
    }

}
