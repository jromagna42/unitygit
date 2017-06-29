using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStorage {

	public static int nextPlayer = 0;
	public static int playerCount = 3;
	public static int startingBoomerang = 10;
	public static int debug = 1;
	public static GameObject[] playersGameObject = new GameObject[playerCount];
	public static int[] playersControlType = new int[playerCount];
	public static int[] playersBoomerangCount = new int[playerCount];
	public static 	GameObject				assetParent;

	public struct PlayerColor
	{
		public Color mainColor;
		public Color altColor;
		
	}

	public static Vector3 tabStartPos;
	public static Dictionary<int, PlayerColor> playerColors = new Dictionary<int, PlayerColor>()
	{
 		{ 0, new PlayerColor {mainColor = Color.red , altColor = Color.magenta}},
 		{ 1, new PlayerColor {mainColor = Color.blue , altColor = Color.cyan}},
		{ 2, new PlayerColor {mainColor = Color.green , altColor = Color.yellow}}
	};

	public static float[,] botTab;
	public static float tabHSize;
	public static float tabVSize;
	public static int tabHNumber = 50;
	public static int tabVNumber = 50 / 16 * 9;
	
}
