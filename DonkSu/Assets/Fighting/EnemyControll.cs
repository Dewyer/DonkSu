using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour
{
    public enum EnemyType
    {
        Robot,Dog,Shield
    }
    public Dictionary<EnemyType,float> MaxHps = new Dictionary<EnemyType, float>(){{EnemyType.Dog,100f},{EnemyType.Robot,200},{EnemyType.Shield,100}};

    public float HP;
    public EnemyType ThisEnemyType;

    public GameObject Shield;

    public GameObject BulletPrefab;
    public GameObject ShootFrom;
    public float BulletDmg;
    public float FireRate;
    public float BulletSpeed;

	// Use this for initialization
	void Start ()
	{
	    HP = MaxHps[ThisEnemyType];

	    if (ThisEnemyType == EnemyType.Robot || ThisEnemyType == EnemyType.Shield)
	    {
	        StartCoroutine(ShootLoop());
	    }
	}

    IEnumerator ShootLoop()
    {
        for (;;)
        {
            yield return new WaitForSeconds(FireRate);

            var bb = (GameObject) Instantiate(BulletPrefab, ShootFrom.transform.position, Quaternion.identity);
            bb.GetComponent<Rigidbody2D>().velocity = transform.right * BulletSpeed;
            var bullet = bb.GetComponent<BulletControll>();
            bullet.Pierce = false;
            bullet.Damage = BulletDmg;

            bb.tag = "EnemyBullet";
        }
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
            Destroy(gameObject);
        }
    }
}
