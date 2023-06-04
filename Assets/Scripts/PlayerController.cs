using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float ascendForce;
    [SerializeField] private float ascendBoostMultiplier;
    [SerializeField] private float horizontalMovementSpeed;
    [SerializeField] private float movementSpeedIncrease;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator deathAnimator;
    [SerializeField] private GameObject trail;

    private Rigidbody2D rb;
    private float timeSinceLevelStart;
    private bool ascend;
    private float ascendBoost;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        animator.SetBool("GameRunning", true);
        deathAnimator.gameObject.SetActive(false);
        trail.SetActive(false);
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
            ascendBoost += Time.deltaTime * ascendBoostMultiplier;
            rb.velocity = new Vector3(horizontalMovementSpeed, ascendForce + ascendBoost);
        }
        else
            ascendBoost = 0;

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
		deathAnimator.gameObject.SetActive(true);
		deathAnimator.SetTrigger("Die");
    }

    public void OnStartGame()
    {
        rb.isKinematic = false;
        trail.SetActive(true);
    }
}
