using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpForce = 8.0f;
    public float turnSpeed;

    private PlayerManager playerManager;
    private Rigidbody rb;
    private Animator animator;
    private Quaternion targetRotation; // The target rotation to turn the player

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rb = playerManager.rb;
        animator = playerManager.animator;
        targetRotation = transform.rotation;
    }

   
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
      // code for Vertical/Forward Movement
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = ( transform.forward * moveZ).normalized * speed * Time.deltaTime;
        animator.SetFloat("Movement", Mathf.Abs( moveZ));
        Vector3 targetPosition = rb.position + move;
        rb.MovePosition(targetPosition);

        //code for Rotating Left and Right
        //only rotate when not selecting gravity direction
        if (playerManager.Hologram.activeInHierarchy ==false)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetRotation *= Quaternion.Euler(0, 90, 0);
                //transform.Rotate(Vector3.up,  turnSpeed * Time.deltaTime);

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                targetRotation *= Quaternion.Euler(0, -90, 0);
                // transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            targetRotation = transform.rotation;
        }

        // Handle jumping but no animation provided
        if (playerManager.IsGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(-Physics.gravity.normalized * jumpForce, ForceMode.Impulse);
        }
    }

}