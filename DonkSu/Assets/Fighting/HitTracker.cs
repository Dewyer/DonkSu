using System.Collections;
using System.Collections.Generic;
using Assets.Fighting;
using UnityEngine;

public class HitTracker : MonoBehaviour
{
    public Controller Controller;

    public HitTrackerEnd HitTrackerEnd;
    public GameObject HitApproacherPrefab;
    public GameObject Beats;

    public Sprite NotHitSprite;
    public Sprite HitSprite;

    public float startWhere;

    public Dictionary<GameObject, HitObject> GOToHitO;

    void Start () {
		GOToHitO = new Dictionary<GameObject, HitObject>();
        HitTrackerEnd.HitTracker = this;
    }

    public void OnHit()
    {
        StartCoroutine(OnHitRutine());
    }

    private IEnumerator OnHitRutine()
    {
        HitTrackerEnd.gameObject.GetComponent<SpriteRenderer>().sprite = HitSprite;
        yield return new WaitForSeconds(0.2f);
        HitTrackerEnd.gameObject.GetComponent<SpriteRenderer>().sprite = NotHitSprite;

    }

    public void StartNewHit(HitObject ho,float time)
    {
        var gg = (GameObject) Instantiate(HitApproacherPrefab, new Vector3(startWhere, 0, 0), Quaternion.identity,Beats.transform);
        gg.transform.parent = Beats.transform;
        gg.transform.position = new Vector3(startWhere,Beats.transform.position.y,0);

        var targetT = HitTrackerEnd.GetComponent<Transform>();

        var dist =(startWhere-gg.transform.localScale.x/2) - (targetT.position.x+(targetT.localScale.x/2));
        var deltaT = time - (ho.AtTime / 1000f);
        gg.GetComponent<Rigidbody2D>().velocity = new Vector2((dist/deltaT),0);

        if (ho.Type == HitObjectType.Slider)
        {
            gg.GetComponent<SpriteRenderer>().color = new Color(1f,0.5f,0,1f);
        }

        GOToHitO.Add(gg,ho);
    }

}
