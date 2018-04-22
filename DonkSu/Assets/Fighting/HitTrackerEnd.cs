using System.Collections;
using System.Collections.Generic;
using Assets.Fighting;
using UnityEngine;

public class HitTrackerEnd : MonoBehaviour
{

    public List<GameObject> ObjectsIn;
    public HitTracker HitTracker;

	void Start ()
	{
	    ObjectsIn = new List<GameObject>();
	}


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (ObjectsIn.IndexOf(other.gameObject) == -1 && other.gameObject.tag == "HitO")
        {
            ObjectsIn.Add(other.gameObject);
        }

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (ObjectsIn.IndexOf(other.gameObject) == -1 && other.gameObject.tag == "HitO")
        {
            ObjectsIn.Add(other.gameObject);
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (ObjectsIn.IndexOf(other.gameObject) != -1 && other.gameObject.tag == "HitO")
        {
            ObjectsIn.Remove(other.gameObject);

            DestroyHit(other.gameObject);
        }
    }

    private void DestroyHit(GameObject otherGameObject)
    {
        HitTracker.GOToHitO.Remove(otherGameObject);
        Destroy(otherGameObject);
    }
}
