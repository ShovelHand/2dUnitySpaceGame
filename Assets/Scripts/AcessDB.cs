using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcessDB : MonoBehaviour {

    string url = "http://localhost/unity/updateScore.php";
    string result;

    string playerName = "Unamed Player";
    // Use this for initialization
    IEnumerator Start () {
        WWW www = new WWW(url);
        yield return www;
        result = www.text;
        print("data received: " + result);


        GameObject.Find("scores").GetComponent<Text>().text = result;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Display()
    {
        GameObject.Find("scores").GetComponent<Text>().text = result;
    }

    public void SetName(string name)
    {
        playerName = name;
        Debug.Log("Player Name: " + playerName);
    }

    public void UpdateDB(int score)
    {
        Debug.Log("Update database");
        StartCoroutine(connectToPHP(score));
    }

    IEnumerator connectToPHP(int score)
    {
        url += "?name=" + playerName + "&score=" + score;
        WWW www = new WWW(url);
        yield return www;
        Debug.Log("DB updated");
    }
}
 