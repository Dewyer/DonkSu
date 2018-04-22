using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private Rigidbody2D rgb;

    private Animator Animator;

    //Pubs
    public float PlayerSpeed;
    private bool _walking;

    public bool Walking {
        get { return _walking; }
        set
        {
            _walking = value;
            Animator.SetBool("Walking",_walking);
        }
    }

    void Start ()
	{
	    rgb = gameObject.GetComponent<Rigidbody2D>();
	    Animator = gameObject.GetComponent<Animator>();
	}
	
	void Update ()
	{
	    var horizontal = Input.GetAxis("Horizontal");
	    var vertical = Input.GetAxis("Vertical");

	    var playerVelocity = new Vector2(horizontal*PlayerSpeed,vertical*PlayerSpeed);
	    rgb.velocity = playerVelocity;

	    if (rgb.velocity.magnitude != 0)
	    {
	        Walking = true;
	    }
	    else
	    {
	        Walking = false;
	    }

	    var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    Vector3 diff = mouse - transform.position;
	    diff.Normalize();

	    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
