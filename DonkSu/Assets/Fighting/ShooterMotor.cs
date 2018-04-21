using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMotor : MonoBehaviour
{
    public GameObject shootFrom;

    public GameObject CirlcePrefab;
    public float CircleSpeed;

    public GameObject SliderPrefab;
    public float SliderSpeed;
    public int SliderFireRate;

    public void ShootCirlce()
    {
        var bb = (GameObject) Instantiate(CirlcePrefab, shootFrom.transform.position, Quaternion.identity);
        bb.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * CircleSpeed;
    }

    public void ShootSlider(int sliderLen)
    {
        var many = (sliderLen/SliderFireRate) ;
        Debug.Log("SL:"+sliderLen);
        StartCoroutine(SliderShots(many));
    }

    IEnumerator SliderShots(int howMany)
    {
        for (int ii = 0; ii < howMany; ii++)
        {
            var bb = (GameObject)Instantiate(SliderPrefab, shootFrom.transform.position, Quaternion.identity);
            bb.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * SliderSpeed;

            yield return new WaitForSeconds(SliderFireRate/1000f);
        }
    }
}
