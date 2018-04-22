using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamager : MonoBehaviour
{

    public float MeleeFireRate;
    public float MeleeDamage;
    public string TargetTag;

    public List<GameObject> InSide;

    public void Start()
    {
        InSide = new List<GameObject>();
        StartCoroutine(DamageLoop());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (InSide.IndexOf(other.gameObject) == -1 && other.gameObject.tag == TargetTag)
        {
            InSide.Add(other.gameObject);
        }
    }

    IEnumerator DamageLoop()
    {
        for (;;)
        {
            yield return new WaitForSeconds(MeleeFireRate);
            InSide.ForEach(x=>x.SendMessage("TakeDamage",MeleeDamage));
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (InSide.IndexOf(other.gameObject) != -1 && other.gameObject.tag == TargetTag)
        {
            InSide.Remove(other.gameObject);
        }
    }
}
