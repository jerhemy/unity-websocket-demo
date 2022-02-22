using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Script from Unity's Roll-a-Ball sample
public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpHeight;

    private Rigidbody rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }
	
    void FixedUpdate() 
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

        if (Input.GetKeyDown("space"))
        {
            Vector3 jump = new Vector3 (0.0f, jumpHeight, 0.0f);
            rb.AddForce (jump);
        }
    }
}