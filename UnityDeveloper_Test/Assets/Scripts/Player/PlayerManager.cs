using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Componenets")]
    public Rigidbody rb;
    public Animator animator;
    // Rotation speed for smoother orientation
    public float rotationSpeed = 5.0f;
    public bool IsGrounded;
    public GameObject Hologram;


    //GRAVITY VARIABLES
    // Current gravity affecting the player
    private Vector3 currentGravity = Vector3.down;
    // Temporary gravity for selection
    private Vector3 selectedGravity;
    // Enums to track gravity state in both vertical and horizontal planes
    private enum VerticalGravityDirection { Forward, Up, Backward, Down }
    private enum HorizontalGravityDirection { Right, Forward, Left, Backward }
    private VerticalGravityDirection currentVerticalGravity = VerticalGravityDirection.Down;
    private HorizontalGravityDirection currentHorizontalGravity = HorizontalGravityDirection.Forward;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        selectedGravity = currentGravity; // Initialize selectedGravity as the starting gravity
        Physics.gravity = currentGravity * 9.81f; // Set initial gravity
    }

    // Update is called once per frame
    void Update()
    {
        // Gravity selection using arrow keys
        SelectGravityDirection();
       
        // Apply selected gravity when Enter is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {          
            currentGravity = selectedGravity;
            Physics.gravity = currentGravity * 9.81f * 5; // Apply new gravity globally
            StartCoroutine(SetHoloGramOff());
        }
        AdjustPlayerOrientation();
    }

    //setting off hologram after an delay for nice effect and to get proper player movement roataion using A and D  
    IEnumerator SetHoloGramOff() 
    {
        yield return new WaitForSeconds(1.1f);
        Hologram.gameObject.SetActive(false);
    }

    #region Gravity Manipulation
    void SelectGravityDirection()
    {
        // Handle gravity direction selection with the arrow keys
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Hologram.gameObject.SetActive(true);
            CycleVerticalGravity();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Hologram.gameObject.SetActive(true);
            CycleHorizontalGravity(true); // Forward cycle for Right Arrow
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Hologram.gameObject.SetActive(true);
            CycleHorizontalGravity(false); // Reverse cycle for Left Arrow
        }





        AdjustHoloGramOrientation(Hologram.transform,selectedGravity);
    }


    // Cycle through vertical gravity directions: Forward -> Up -> Backward -> Down
    private void CycleVerticalGravity()
    {
        switch (currentVerticalGravity)
        {
            case VerticalGravityDirection.Forward:
                currentVerticalGravity = VerticalGravityDirection.Up;
                selectedGravity = transform.up; //Vector3.up;
                break;

            case VerticalGravityDirection.Up:
                currentVerticalGravity = VerticalGravityDirection.Backward;
                selectedGravity = -transform.forward; // Backward relative to the player
                break;

            case VerticalGravityDirection.Backward:
                currentVerticalGravity = VerticalGravityDirection.Down;
                selectedGravity = -transform.up; //Vector3.down;
                break;

            case VerticalGravityDirection.Down:
                currentVerticalGravity = VerticalGravityDirection.Forward;
                selectedGravity = transform.forward; // Forward relative to the player
                break;
        }
    }

    // Cycle through horizontal gravity directions: Right -> Forward -> Left -> Backward (for Right Arrow)
    private void CycleHorizontalGravity(bool isRightArrow)
    {
        if (isRightArrow)
        {
            // Cycle forward through horizontal directions for Right Arrow
            switch (currentHorizontalGravity)
            {
                case HorizontalGravityDirection.Right:
                    currentHorizontalGravity = HorizontalGravityDirection.Forward;
                    selectedGravity = transform.forward;
                    break;

                case HorizontalGravityDirection.Forward:
                    currentHorizontalGravity = HorizontalGravityDirection.Left;
                    selectedGravity = -transform.right; // Left relative to the player
                    break;

                case HorizontalGravityDirection.Left:
                    currentHorizontalGravity = HorizontalGravityDirection.Backward;
                    selectedGravity = -transform.forward; // Backward relative to the player
                    break;

                case HorizontalGravityDirection.Backward:
                    currentHorizontalGravity = HorizontalGravityDirection.Right;
                    selectedGravity = transform.right; // Right relative to the player
                    break;
            }
        }
        else
        {
            // Reverse cycle for Left Arrow
            switch (currentHorizontalGravity)
            {
                case HorizontalGravityDirection.Right:
                    currentHorizontalGravity = HorizontalGravityDirection.Backward;
                    selectedGravity = -transform.forward;
                    break;

                case HorizontalGravityDirection.Backward:
                    currentHorizontalGravity = HorizontalGravityDirection.Left;
                    selectedGravity = -transform.right;
                    break;

                case HorizontalGravityDirection.Left:
                    currentHorizontalGravity = HorizontalGravityDirection.Forward;
                    selectedGravity = transform.forward;
                    break;

                case HorizontalGravityDirection.Forward:
                    currentHorizontalGravity = HorizontalGravityDirection.Right;
                    selectedGravity = transform.right;
                    break;
            }
        }
    } 

    void AdjustPlayerOrientation()
    {
        // Calculate the new "up" direction for the player, opposite to the current gravity
        Vector3 targetUp = -Physics.gravity.normalized;      
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, targetUp) * transform.rotation;        
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }

    void AdjustHoloGramOrientation( Transform target ,Vector3 newUp)
    {
        // Calculate the new "up" direction for the player, opposite to the current gravity
        Vector3 targetUp = -newUp.normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(target.up, targetUp) * target.rotation;
        target.rotation = targetRotation;
       
    }

    #endregion

    #region collisioncheck
    // Detect if the player is grounded
    void OnCollisionStay(Collision collision)
    {
        IsGrounded = true;
        animator.SetBool("Falling", false);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
        animator.SetBool("Falling", true);
    }
    #endregion
}
