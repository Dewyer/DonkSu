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
        var bullet = bb.GetComponent<BulletControll>();
        bullet.Damage = 300f;
        bullet.Pierce = true;

    }

    public void ShootSlider(int sliderLen)
    {
        var many = (sliderLen/SliderFireRate) ;

        StartCoroutine(SliderShots(many));
    }

    IEnumerator SliderShots(int howMany)
    {
        for (int ii = 0; ii < howMany; ii++)
        {
            var bb = (GameObject)Instantiate(SliderPrefab, shootFrom.transform.position, Quaternion.identity);
            bb.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * SliderSpeed;
            var bullet = bb.GetComponent<BulletControll>();
            bullet.Damage = 200f;
            bullet.Pierce = false;

            yield return new WaitForSeconds(SliderFireRate/1000f);
        }
    }
}
