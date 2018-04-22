using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControll : MonoBehaviour
{

    public float Damage { get; set; }
    public bool Pierce { get; set; }

    void Start () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Shield" && this.tag != "EnemyBullet")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Enemy" && this.tag != "EnemyBullet")
        {
            other.gameObject.SendMessage("TakeDamage",Damage);
            if (!Pierce)
            {
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag == "Player" && this.tag == "EnemyBullet")
        {
            other.gameObject.SendMessage("TakeDamage", Damage);
            Destroy(gameObject);
        }
    }


}
