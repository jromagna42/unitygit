using UnityEngine;
using System.Collections;

public class loader : MonoBehaviour {

	public GameObject gamemanager;
	// Use this for initialization
	void awake () 
	{
		print("le reveil loader");
		if (GameManager.instance == null)
		{
			Instantiate(gamemanager);
		}
	}
}
