using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundSetter : MonoBehaviour
{

    public Collider2D Up;
    public Collider2D Down;
    public Collider2D Left;
    public Collider2D Right;


    void Start ()
    {
		var screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height));

        Up.transform.localScale = new Vector3(screenSize.x*2,1,1);
        Down.transform.localScale = new Vector3(screenSize.x*2,0.1f,0.1f);
        Up.transform.position = new Vector3(0,screenSize.y+0.5f);
        Down.transform.position = new Vector3(0, -screenSize.y- 0.05f+2f);

        Left.transform.localScale = new Vector3(1,screenSize.y*2,1);
        Right.transform.localScale = Left.transform.localScale;

        Left.transform.position = new Vector3(-screenSize.x - 0.5f, 0);
        Right.transform.position = new Vector3(screenSize.x + 0.5f, 0);

    }

}
