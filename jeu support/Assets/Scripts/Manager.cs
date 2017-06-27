using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	const string			assetParentName = "SceneAssets";
	// Use this for initialization

	int tabVNumber = 50;
	int tabHNumber = 50;
	public float cameraSize;
	public float cameraSizeV;
	public Vector3 tabStartPos; 
	void Start () {
		cameraSizeV = (Camera.main.orthographicSize * 2);
		cameraSize = ((cameraSize / 9) * 16);
		DataStorage.camHSize = cameraSize / 16;
		DataStorage.camVSize = cameraSizeV / 9;
		if ((DataStorage.assetParent = GameObject.Find(assetParentName)) == null)
			DataStorage.assetParent = new GameObject(assetParentName);
		DataStorage.botTab = new float[tabHNumber, tabVNumber];
	}

	// Update is called once per frame
	
	void ShowGrid()
	{
		int i = 0;
		tabStartPos = new Vector3((Camera.main.transform.position.x - cameraSize), 0, (Camera.main.transform.position.z - cameraSizeV));
		Vector3 tabMod = new Vector3(DataStorage.camHSize,0 , 0);
		
	//	while (i < tabHNumber)
	//	{
			Debug.DrawLine(tabStartPos + (i * tabMod), tabStartPos + (i * tabMod) + new Vector3(0, 0, cameraSize), Color.green);
			//i++;
	//	}
		i = 0;
		tabMod = new Vector3(0 ,0 , DataStorage.camVSize);
	//	while (i < tabHNumber)
	//	{
			Debug.DrawLine(tabStartPos + (i * tabMod), tabStartPos + (i * tabMod) + new Vector3(cameraSizeV, 0, 0), Color.black);
			//	i++;
	//	}

	}
	
	void Update () {
		if (DataStorage.debug == 1)
			ShowGrid();
		Debug.DrawLine(Vector3.zero , new Vector3(5, 0, 0), Color.red);
		Debug.DrawLine(Vector3.zero , new Vector3(0, 5, 0), Color.green);
		Debug.DrawLine(Vector3.zero , new Vector3(0, 0, 5), Color.blue);
		if (Input.GetKeyDown(KeyCode.Return))
		{
			DataStorage.nextPlayer = 0;
			SceneManager.LoadScene("main", LoadSceneMode.Single);
		}
	}
}
