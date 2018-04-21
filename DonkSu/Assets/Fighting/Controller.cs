using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Fighting;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public AudioSource source;

    public string LevelName;
    public string MapName;
    private string basePath;
    public HitTracker HitTracker;

    public ShooterMotor Shooter;

    public AudioClip clip;
    private OsuFile osuMap;

    public bool MapIsGoing;

	void Start ()
	{
        StartMap();
	}

    public void StartMap()
    {
        MapIsGoing = true;

        basePath = "/" + LevelName + "/";
        osuMap = new OsuFile("Assets/Resources" + basePath+MapName+".osu");

        var bites = File.ReadAllBytes("Assets/Resources" + basePath + osuMap.PropertiesDictionary["AudioFilename"]);

        clip = NAudioPlayer.FromMp3Data(bites);
        source.clip = clip;
        source.Play();

    }

    public void StartNewHits()
    {
        if (osuMap.IsTheNextBeatComingUp(source.time))
        {
            var ht = osuMap.GetNextHitObject();

            HitTracker.StartNewHit(ht,source.time);
        }
    }

    private void TryFire()
    {
        if (HitTracker.HitTrackerEnd.ObjectsIn.Count > 0)
        {
            var hot = HitTracker.GOToHitO[HitTracker.HitTrackerEnd.ObjectsIn[0]];

            if (hot.Type == HitObjectType.Circle)
            {
                Shooter.ShootCirlce();
            }
            else if (hot.Type == HitObjectType.Slider)
            {
                Shooter.ShootSlider(hot.Length);
            }
        }
    }


    void Update ()
    {
        if (MapIsGoing)
        {
            if (osuMap == null)
                return;

            StartNewHits();

            var down = Input.GetMouseButtonDown(0);
            Debug.Log(down);
            if (down)
            {
                TryFire();
            }
        }
    }

}
