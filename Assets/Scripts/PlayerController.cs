using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float tapForce;
    [SerializeField] private float horizontalMovementSpeed;
    [SerializeField] private float movementSpeedIncrease;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private bool pendingTap;
    private float timeSinceLevelStart;
    private bool ascend;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        animator.SetBool("GameRunning", true);
    }

    private void Update()
    {
        if (!GameManager.Instance.GameRunning)
            return;

        ascend = Input.GetMouseButton(0);
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.GameRunning)
            return;

        if (ascend)
        {
            rb.velocity = new Vector3(horizontalMovementSpeed, tapForce);
        }

        timeSinceLevelStart += Time.fixedDeltaTime;
        var increase = timeSinceLevelStart * movementSpeedIncrease;
        var movementSpeed = horizontalMovementSpeed + increase;

        rb.velocity = new Vector3(movementSpeed, rb.velocity.y);
        //Debug.Log($"multiplier: {increase}, movementSpeed: {movementSpeed}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.GameRunning)
            return;

        Debug.Log("Game over");
        GameManager.Instance.GameOver();
        animator.SetBool("GameRunning", false);
    }

    public void OnStartGame()
    {
        rb.isKinematic = false;
    }
}
