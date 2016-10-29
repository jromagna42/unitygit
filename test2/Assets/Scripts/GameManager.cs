using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;
	private BoardManager boardScript;
	private int level = 3;
	public int playerfoodpoint = 100;
	[HideInInspector] public bool playerturn = true;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		boardScript = GetComponent<BoardManager>();

		InitGame();
	}

	void InitGame()
	{
		boardScript.SetupScene(level);

	}

	public void gameover()
	{
		enabled = false;
	}

	void Update()
	{

	}
}
// using UnityEngine;
// using System.Collections;

// public class gamemanager : MonoBehaviour {

// 	public boardmanager boardscript;

// 	private int level = 3;

// 	// Use this for initialization
// 	void awake ()
// 	 {
// 		boardscript = GetComponent<boardmanager>();
// 		initgame();
// 	}

// 	void initgame()
// 	{
// 		boardscript.SetupScene(level);
// 	}

// 	// Update is called once per frame
// 	void Update () 
// 	{
// 	}
// }
