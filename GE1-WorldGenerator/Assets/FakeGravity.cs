using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGravity : MonoBehaviour
{
    public GameObject gravityObject;
    public float gravityAmount = -30;
    public Rigidbody rb; 
    
    
    public float moveSpeed = 0.0f;
    public float rotSpeed = 180.0f;
    
    public float walkSpeed = 6.0f;
    public float RunSpeed = 14.0f;
    public float maxSpeed = 0.0f;
    public float slerpSpeed = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
        if (rb == null)
        {
            rb = transform.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        Vector3 gravityUp = (this.transform.position - gravityObject.transform.position).normalized;
        rb.AddForce(gravityUp*gravityAmount);//add gravity
        Vector3 objUp = transform.up;

        Quaternion targetRotation = Quaternion.FromToRotation(objUp,gravityUp)*transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,slerpSpeed*Time.deltaTime);
        
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
        rb.AddForce(transform.forward*moveSpeed);
        transform.Rotate(-rotateSpeed * Time.deltaTime ,0,0 );

    }
}
