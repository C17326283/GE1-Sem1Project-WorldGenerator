using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    CharacterController characterController;

    public float moveSpeed = 0.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float rotSpeed = 180.0f;
    
    public float walkSpeed = 6.0f;
    public float RunSpeed = 14.0f;
    public float maxSpeed = 0.0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 rotation;
     
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        this.rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotSpeed * Time.deltaTime, 0);//get new rotation
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));
            moveDirection = this.transform.TransformDirection(moveDirection) * moveSpeed;

            if (Input.GetAxis("Vertical") != 0)//If input then move
            {
                Debug.Log("forward");
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    maxSpeed = RunSpeed;
                }
                else
                {
                    maxSpeed = walkSpeed;
                }
            
                if(moveSpeed < maxSpeed)//get faster untill you reach desired speed
                {
                    moveSpeed += 1.0f;//get faster
                }
                else if (moveSpeed > maxSpeed)
                {
                    moveSpeed -= 1.0f;
                }
            }
            else//if stop pressing then slow down
            {
                if(moveSpeed > 0.0f || moveSpeed > maxSpeed)//stop slowing at 0
                {
                    moveSpeed = 0.0f;
                }
            }
        
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        this.transform.Rotate(this.rotation);
    }//End update
}
