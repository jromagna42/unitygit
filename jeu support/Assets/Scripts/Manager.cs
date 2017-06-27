using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	const string			assetParentName = "SceneAssets";
	// Use this for initialization
	void Start () {
		int cameraSize = (int)(Camera.main.orthographicSize * 2);
		int	cameraSizeV = (cameraSize / 16 * 9);
		DataStorage.camHSize = cameraSize / 16;
		DataStorage.camVsize = cameraSizeV / 9;
		if ((DataStorage.assetParent = GameObject.Find(assetParentName)) == null)
			DataStorage.assetParent = new GameObject(assetParentName);
		DataStorage.botTab = new int[cameraSize * 2, cameraSizeV * 2];
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			DataStorage.nextPlayer = 0;
			SceneManager.LoadScene("main", LoadSceneMode.Single);
		}
	}
}
