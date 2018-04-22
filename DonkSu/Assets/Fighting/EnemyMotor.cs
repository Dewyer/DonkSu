using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{

    public float Speed;
    public float TurnRate;
    public bool Dead;

    public Transform Target;
	// Use this for initialization
	void Start ()
	{
	    Target = GameObject.FindGameObjectWithTag("Player").transform;
	    Dead = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Dead)
	    {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
            return;
	        
	    }

	    Vector3 vectorToTarget = Target.position - transform.position;
	    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        
	    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
	    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * TurnRate);

	    var rgb = gameObject.GetComponent<Rigidbody2D>();
	    rgb.velocity = transform.right * Speed;

	}
}
