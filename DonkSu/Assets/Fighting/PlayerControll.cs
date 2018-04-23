using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{

    private float _hp;
    public GameObject HearthCollector;
    public GameObject HearthPrefab;
    public List<GameObject> Hearths;

    public float HP
    {
        get { return _hp;}
        set
        {
            _hp = value;
            var NoH = (int)Mathf.Round(HP / 100f);

            SetHearthCount(NoH);
        }

    }

    public void SetHearthCount(int many)
    {
        Hearths.ForEach(x=>Destroy(x));
        Hearths.Clear();

        for (int ii = 0; ii < many; ii++)
        {
            var hh = (GameObject) Instantiate(HearthPrefab, new Vector3(ii * 0.47f, 0, 0), Quaternion.identity,
                HearthCollector.transform);
            Hearths.Add(hh);

            hh.transform.localPosition = new Vector3(ii*0.47f,0,0);
        }
    }

    // Use this for initialization
	void Start ()
	{
	    HP = 800;
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
            GameObject.FindGameObjectWithTag("Controller").gameObject.GetComponent<Controller>().GameEnded();
        }
    }
}
