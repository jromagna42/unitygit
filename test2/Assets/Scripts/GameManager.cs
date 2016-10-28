
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

		 public static GameManager instance = null;      
        private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
        private int level = 3;                                  //Current level number, expressed in game as "Day 1".

        //Awake is always called before any Start functions
        void Awake()
        {print("le reveil gamemanager");
             if (instance == null)
                
                //if not, set instance to this
                instance = this;

                else if (instance != this)
                
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);  
                 DontDestroyOnLoad(gameObject);
            //Get a component reference to the attached BoardManager script
            boardScript = GetComponent<BoardManager>();
            
            //Call the InitGame function to initialize the first level 
            InitGame();
        }
        
        //Initializes the game for each level.
        void InitGame()
        {
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);
            
        }
        
        
        
        //Update is called every frame.
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
