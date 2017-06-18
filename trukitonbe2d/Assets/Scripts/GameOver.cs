using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour 
{

	public GameObject gameOverScreen;
	public Text secondSurvivedUI;
	// Use this for initialization
	bool gameOver;
	void Start () 
	{
		FindObjectOfType<PlayerControler>().OnPlayerDeath += OnGameOver;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gameOver)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				SceneManager.LoadScene(0);
			}
		}
	}

	void OnGameOver() 
	{
		gameOverScreen.SetActive (true);
		secondSurvivedUI.text = Mathf.RoundToInt(Time.timeSinceLevelLoad).ToString();
		gameOver = true;
	}
}

