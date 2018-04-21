using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private Rigidbody2D rgb;

    //Pubs
    public float PlayerSpeed;


	void Start ()
	{
	    rgb = gameObject.GetComponent<Rigidbody2D>();

	}
	
	void Update ()
	{
	    var horizontal = Input.GetAxis("Horizontal");
	    var vertical = Input.GetAxis("Vertical");

	    var playerVelocity = new Vector2(horizontal*PlayerSpeed,vertical*PlayerSpeed);
	    rgb.velocity = playerVelocity;

	    var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    Vector3 diff = mouse - transform.position;
	    diff.Normalize();

	    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
