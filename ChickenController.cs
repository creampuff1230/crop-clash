using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    // controls chicken movements -- they walk with animation, stop,
    // change direction, based on timers
    // animation is based on finite state machine in the chicken
    // animator controller
    public float moveSpeed = 1f; 
    public float changeDirectionInterval = 3f; 
    public float stopInterval = 4f; 

    private Animator animator;
    private Rigidbody rb;
    private Vector3 direction;
    private float timer;
    private float stopTimer;
    private bool isWalking;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); 
        ChangeDirection(); // set initial random direction
        timer = changeDirectionInterval;
        stopTimer = stopInterval;
        isWalking = true; 
    }

    void Update()
    {
        // use rigidbody to move chickens
        if (isWalking)
        {
            rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * direction); // Move using Rigidbody
        }

        // update timers
        timer -= Time.deltaTime;
        stopTimer -= Time.deltaTime;

        // change direction when the timer reaches 0
        if (timer <= 0)
        {
            ChangeDirection();
            timer = changeDirectionInterval; // reset timer
        }

        // stop the chicken for a random time before walking again
        if (stopTimer <= 0 && isWalking)
        {
            isWalking = false; 
            stopTimer = stopInterval; // reset stop timer
        }

        // resume walking if stop time is up
        if (!isWalking && stopTimer <= 0)
        {
            isWalking = true; 
            ChangeDirection(); // change direction upon start
            stopTimer = stopInterval; // reset the stop timer
        }

        // set animation parameter (except i think this is working backwards rn but
        // its kind of cute the way it is
        if (animator != null)
        {
            float speed = isWalking ? moveSpeed : 0f; // set speed to 0 if not walking
            animator.SetFloat("speed", speed); 
        }
    }

    private void ChangeDirection()
    {
        // get random direction to change direction to
        float angle = Random.Range(0f, 360f);
        direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
