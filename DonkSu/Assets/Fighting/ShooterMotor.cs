using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMotor : MonoBehaviour
{
    public GameObject shootFrom;

    public GameObject MuzzleFlash;
    public DateTime FlashTill;

    public GameObject CirlcePrefab;
    public float CircleSpeed;

    public GameObject SliderPrefab;
    public float SliderSpeed;
    public int SliderFireRate;

    public void ShootCirlce()
    {
        var bb = (GameObject) Instantiate(CirlcePrefab, shootFrom.transform.position, Quaternion.identity);
        bb.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * CircleSpeed;
        bb.transform.rotation = gameObject.transform.rotation;

        var bullet = bb.GetComponent<BulletControll>();
        bullet.Damage = 300f;
        bullet.Pierce = true;

        FlashTill = DateTime.Now.AddSeconds(0.4f);
        StartCoroutine(SetMuzzleFlash());
    }

    public void ShootSlider(int sliderLen)
    {
        var many = (sliderLen/SliderFireRate) ;
        FlashTill = DateTime.Now.AddSeconds(many * (SliderFireRate / 1000f));
        StartCoroutine(SetMuzzleFlash());

        StartCoroutine(SliderShots(many));
    }

    public IEnumerator SetMuzzleFlash()
    {
        MuzzleFlash.SetActive(true);

        yield return new WaitForSeconds((float)((FlashTill-DateTime.Now).TotalSeconds));

        if (DateTime.Now >= FlashTill)
        {
            MuzzleFlash.SetActive(false);
        }
    }

    IEnumerator SliderShots(int howMany)
    {
        for (int ii = 0; ii < howMany; ii++)
        {
            var bb = (GameObject)Instantiate(SliderPrefab, shootFrom.transform.position, Quaternion.identity);
            bb.transform.rotation = gameObject.transform.rotation;
            bb.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * SliderSpeed;
            var bullet = bb.GetComponent<BulletControll>();
            bullet.Damage = 200f;
            bullet.Pierce = false;

            yield return new WaitForSeconds(SliderFireRate/1000f);
        }
    }
}
