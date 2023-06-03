using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, target.position, Time.deltaTime * speed);
    }
}
