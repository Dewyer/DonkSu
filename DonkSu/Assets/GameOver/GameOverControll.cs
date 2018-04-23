using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverControll : MonoBehaviour
{

    public Text BigText;

    public Text InfoText;

	// Use this for initialization
	void Start ()
	{
	    var coins = PlayerPrefs.GetInt("coinsCollected");
	    var maxCombo = PlayerPrefs.GetInt("maxCombo");
	    var won = PlayerPrefs.GetInt("won") == 1;

	    if (won)
	    {
	        BigText.text = "Good Job!";
            BigText.color = new Color(0,1f,0,1f);
	    }
	    else
	    {
	        BigText.text = "Game Over!";
	        BigText.color = Color.white;
	    }

	    InfoText.text = "Coins: " + coins + ", Max Combo: x" + maxCombo;
	}

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
