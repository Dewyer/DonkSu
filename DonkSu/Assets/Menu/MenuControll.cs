using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControll : MonoBehaviour
{
    public GameObject Container;
    public GameObject MapPrefab;

    private List<Map> Maps;

    void Start ()
    {
        var mapContents = File.ReadAllText("Assets/Resources/info.json");
        Maps = JsonConvert.DeserializeObject<List<Map>>(mapContents);

        StartCoroutine(CreateMaps());
    }

    private IEnumerator CreateMaps()
    {
        yield return new WaitForEndOfFrame();
        var ii = 0;

        foreach (var map in Maps)
        {
            var newMap = (GameObject) Instantiate(MapPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var rect = newMap.GetComponent<RectTransform>();
            rect.parent = Container.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(30,-30+(-60*ii),0);

            var pa = Path.GetFileName(map.MapName).Split('.')[0];
            if (pa.Length > 35)
            {
                pa = pa.Substring(0, 32);
                pa += "...";
            }

            newMap.GetComponent<Text>().text = pa+", "+map.Stars+" stars";
            var vals = ii;
            newMap.GetComponentInChildren<Button>().onClick.AddListener(delegate { ClickedPlay(vals);});
            ii++;

        }
    }

    public void ClickedPlay(int ii)
    {
        Debug.Log(ii);
        PlayerPrefs.SetString("lvlname",Maps[ii].LevelName);
        PlayerPrefs.SetString("mapname", Maps[ii].MapName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Fighting");
    }
}
