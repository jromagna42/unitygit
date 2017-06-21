using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStorage {

	public static int nextPlayer = 0;
	public static int playerCount = 2;
	public static int startingBoomerang = 1000;
	public static int debug = 1;
	public static GameObject[] playersGameObject = new GameObject[playerCount];
	public static int[] playersControlType = new int[playerCount];
	public static int[] playersBoomerangCount = new int[playerCount];
	public static 	GameObject				assetParent;

	public static Dictionary<int, Color> playerColors = new Dictionary<int, Color>()
	{
 	   { 0, Color.red},
 	   { 1, Color.blue}
	};

}
