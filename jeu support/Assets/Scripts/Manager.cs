using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {


	const string			assetParentName = "SceneAssets";
	// Use this for initialization
	void Start () {
		if ((DataStorage.assetParent = GameObject.Find(assetParentName)) == null)
			DataStorage.assetParent = new GameObject(assetParentName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
