using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{

    public float HP;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float dmg)
    {
        if (HP >= dmg)
        {
            HP -= dmg;
        }
        else if (HP < dmg)
        {
            HP = 0;
        }

        if (HP == 0f)
        {
            //DEAD
            Debug.Log("Dead!");

        }
    }
}
