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
    public GameObject BasicEnemy;

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

            StartCoroutine(StartEnemySpawn());
        }
    }

    private IEnumerator StartEnemySpawn()
    {
        yield return new WaitForSeconds(0.8f);
        SpawnNewEnemy();
    }

    private void SpawnNewEnemy()
    {
        if (Random.Range(0,100) <= 60)
            return;

        var type = Random.Range(0, 100);
        var gg = SpawnEnemyToRandomLocation();
        var ec = gg.GetComponent<EnemyControll>();

        if (type <= 20)
        {
            //DOG
            ec.ThisEnemyType = EnemyControll.EnemyType.Dog;

        }
        else if (type <= 80)
        {
            //robot
            ec.ThisEnemyType = EnemyControll.EnemyType.Robot;
        }
        else
        {
            //Shield
            ec.ThisEnemyType = EnemyControll.EnemyType.Shield;
            ec.Shield.SetActive(true);

        }
        SetEnemyLooks(gg, ec);
    }

    private void SetEnemyLooks(GameObject gg, EnemyControll ec)
    {
        if (ec.ThisEnemyType == EnemyControll.EnemyType.Dog)
        {
            gg.GetComponent<SpriteRenderer>().color = new Color(0.4f,0.2f,0.3f,1f);
        }
        else if (ec.ThisEnemyType == EnemyControll.EnemyType.Shield)
        {
            gg.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.3f, 0.3f, 1f);

        }
    }

    public GameObject SpawnEnemyToRandomLocation()
    {
        var screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        var pos = Random.Range(0.1f, 0.9f);
        var realPos = new Vector3(0,0,0);
        var side = Random.Range(0, 4);
        var spacing = 1f;

        if (side == 0)
        {
            //UP
            realPos = new Vector3(pos*screenSize.x,screenSize.y+ spacing, 0);
        }
        else if (side == 1)
        {
            //DOWN
            realPos = new Vector3(pos * screenSize.x, -screenSize.y - spacing, 0);
        }
        else if (side == 2)
        {
            //Left
            realPos = new Vector3(-screenSize.x-spacing,screenSize.y*pos, 0);

        }
        else
        {
            //Right
            realPos = new Vector3(screenSize.x + spacing, screenSize.y * pos, 0);

        }

        var gg = (GameObject) Instantiate(BasicEnemy, realPos, Quaternion.identity);

        return gg;
    }

    private void TryFire()
    {
        if (HitTracker.HitTrackerEnd.ObjectsIn.Count > 0)
        {
            var ga = HitTracker.HitTrackerEnd.ObjectsIn[0];
            if (HitTracker.GOToHitO.ContainsKey(ga))
            {
                var hot = HitTracker.GOToHitO[ga];

                if (hot.Type == HitObjectType.Circle)
                {
                    Shooter.ShootCirlce();
                }
                else if (hot.Type == HitObjectType.Slider)
                {
                    Shooter.ShootSlider(hot.Length);
                }

                HitTracker.HitTrackerEnd.ObjectsIn.RemoveAt(0);
                HitTracker.GOToHitO.Remove(ga);
                Destroy(ga);
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

            var down = Input.GetKeyDown(KeyCode.Space);
            if (down)
            {
                TryFire();
            }
        }
    }

}
