using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float tapForce;
    [SerializeField] private float horizontalMovementSpeed;
    [SerializeField] private float movementSpeedIncreaseMultiplier;

    private Rigidbody2D rb;
    private bool pendingTap;
    private float timeSinceLevelStart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!GameManager.Instance.GameRunning)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pendingTap = true;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.GameRunning)
            return;

        if (pendingTap)
        {
            rb.velocity = new Vector3(horizontalMovementSpeed, tapForce);
            pendingTap = false;
        }

        timeSinceLevelStart += Time.fixedDeltaTime;
        var movementSpeed = Mathf.Max(horizontalMovementSpeed, horizontalMovementSpeed * (timeSinceLevelStart * movementSpeedIncreaseMultiplier));

        rb.velocity = new Vector3(movementSpeed, rb.velocity.y);
        Debug.Log(movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Game over");
        GameManager.Instance.GameOver();
    }
}
