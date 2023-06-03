using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float tapForce;
    [SerializeField] private float horizontalMovementSpeed;

    private Rigidbody2D rb;
    private bool pendingTap;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pendingTap = true;
        }
    }

    private void FixedUpdate()
    {
        if (pendingTap)
        {
            rb.AddForce(Vector2.up * tapForce);
            pendingTap = false;
        }

        rb.velocity = new Vector3(horizontalMovementSpeed, rb.velocity.y);
    }
}
