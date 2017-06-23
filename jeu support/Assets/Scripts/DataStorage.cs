using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStorage {

	public static int nextPlayer = 0;
	public static int playerCount = 2;
	public static int startingBoomerang = 2;
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
	public static Dictionary<int, PlayerColor> playerColors = new Dictionary<int, PlayerColor>()
	{
 	   { 0, new PlayerColor {mainColor = Color.red , altColor = Color.yellow}},
 	   { 1, new PlayerColor {mainColor = Color.blue , altColor = Color.cyan}}
	};

}
