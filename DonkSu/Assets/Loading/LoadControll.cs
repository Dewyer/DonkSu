using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Fighting;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Map
{
    public string LevelName { get; set; }
    public string MapName { get; set; }
    public float Stars { get; set; }

}

public class LoadControll : MonoBehaviour
{

    public Text Loading;
    public Text Error;

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(LoadingRutine());
	    StartCoroutine(DotDot());
	}

    private IEnumerator DotDot()
    {
        var at = 1;
        for (;;)
        {
            yield return new WaitForSeconds(0.6f);
            at++;
            at %= 4;
            var dots = "";
            for (int i = 0; i < at; i++)
            {
                dots += ".";
            }
            Loading.text = "Loading"+dots;
        }
    }

    private IEnumerator LoadingRutine()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            var maps = new List<Map>();
            var dirs = Directory.GetDirectories("./Assets/Resources/");

            foreach (var dir in dirs)
            {
                var files = Directory.GetFiles(dir, "*.osu");

                foreach (var file in files)
                {
                    var map = new Map() { LevelName = dir, MapName = file, Stars = 0 };

                    var osu = new OsuFile(file);
                    var dif = float.Parse(osu.PropertiesDictionary["OverallDifficulty"]);
                    map.Stars = dif;
                    maps.Add(map);
                }
            }

            var ss = JsonConvert.SerializeObject(maps);
            File.WriteAllText("Assets/Resources/info.json", ss);
            //DONE
            SceneManager.LoadScene("Menu");
        }
        catch (Exception e)
        {
            Error.text = e.Message;
        }
    }

}
