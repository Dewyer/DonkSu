using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Fighting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{

    public AudioSource source;

    public string LevelName;
    public string MapName;
    private string basePath;
    public HitTracker HitTracker;

    public ShooterMotor Shooter;
    public GameObject BasicEnemy;
    public Sprite ShildedSprite;
    public Sprite RobotSprite;

    public int Combo;
    public int Coins;
    public Text SongName;
    public Text ScoreText;
    public Text Progress;

    public AudioClip clip;
    private OsuFile osuMap;

    public bool MapIsGoing;

	void Start ()
	{
        StartMap();
        UpdateScores();

	    SongName.text = MapName;
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

        Coins = 0;
        Combo = 1;

        StartCoroutine(ProgressTimer());

    }

    private List<string> SecondsToMinutesSeconds(float seconds)
    {
        var ss = (int) seconds;
        var secs = ss % 60;
        var minutes = (ss - secs) / 60;
        var secsS = secs.ToString();
        if (secs < 10)
        {
            secsS = "0" + secs.ToString();
        }

        return new List<string>(){minutes.ToString(),secsS};
    }

    private IEnumerator ProgressTimer()
    {

        var endM = SecondsToMinutesSeconds(source.clip.length);
        while (source.isPlaying)
        {
            var dt = SecondsToMinutesSeconds(source.time);

            Progress.text = String.Format("{0}:{1}/{2}:{3}", dt[0], dt[1], endM[0], endM[1]);
            yield return new WaitForSeconds(0.5f);
        }
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

    private void UpdateScores()
    {
        ScoreText.text = String.Format("{0} x{1}",Coins,Combo);
    }

    private void Killed(EnemyControll.EnemyType type)
    {
        var baseCoins = new Dictionary<EnemyControll.EnemyType,int>() {{ EnemyControll.EnemyType.Robot, 2 },{ EnemyControll.EnemyType.Shield, 5 },{ EnemyControll.EnemyType.Dog, 1 } };

        Coins += baseCoins[type] * Combo;
        UpdateScores();
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
            ec.Biter.SetActive(true);

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
        //gg.GetComponent<Animator>().enabled = false;
        gg.GetComponent<SpriteRenderer>().sprite = RobotSprite;


        if (ec.ThisEnemyType == EnemyControll.EnemyType.Dog)
        {
            gg.GetComponent<Animator>().enabled = true;
            gg.GetComponent<Animator>().SetBool("DogWalk",true);

        }
        else if (ec.ThisEnemyType == EnemyControll.EnemyType.Shield)
        {
            gg.GetComponent<SpriteRenderer>().sprite = ShildedSprite;
        }
    }

    public GameObject SpawnEnemyToRandomLocation()
    {
        var screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        var pos = Random.Range(0.1f, 0.9f);
        var realPos = new Vector3(0,0,0);
        var side = Random.Range(0, 3);
        var spacing = 1f;

        if (side == 0)
        {
            //UP
            realPos = new Vector3(pos*screenSize.x,screenSize.y+ spacing, 0);
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
            HitTracker.OnHit();

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

            Combo++;
        }
        else
        {
            Combo = 1;
        }

        UpdateScores();
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
