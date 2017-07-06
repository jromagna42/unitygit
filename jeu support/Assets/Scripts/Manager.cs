using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	const string			assetParentName = "SceneAssets";
	// Use this for initialization
	public float cameraSize;
	public float cameraSizeV;
	void Start () {
		cameraSizeV = (Camera.main.orthographicSize * 2);
		cameraSize = ((cameraSizeV / 9) * 16);
		DataStorage.tabHSize = cameraSize / DataStorage.tabHNumber;
		DataStorage.tabVSize = cameraSizeV / DataStorage.tabVNumber;
		if ((DataStorage.assetParent = GameObject.Find(assetParentName)) == null)
			DataStorage.assetParent = new GameObject(assetParentName);
		DataStorage.botTab = new float[DataStorage.tabHNumber, DataStorage.tabVNumber];
	}

	// Update is called once per frame
	
	void ShowGrid()
	{
		int j = 0;
		int i = 0;
		DataStorage.tabStartPos = new Vector3((Camera.main.transform.position.x - cameraSize / 2), 0, (Camera.main.transform.position.z - cameraSizeV / 2));
		Vector3 tabMod = new Vector3(DataStorage.tabHSize,0 , 0);
		
		while (i < DataStorage.tabHNumber)
		{
			Debug.DrawLine( DataStorage.tabStartPos + (i * tabMod), DataStorage.tabStartPos + (i * tabMod) + new Vector3(0, 0, cameraSizeV), Color.green);
			i++;
		}
		i = 0;
		tabMod = new Vector3(0 ,0 , DataStorage.tabVSize);
		while (i < DataStorage.tabVNumber)
		{
			Debug.DrawLine( DataStorage.tabStartPos + (i * tabMod), DataStorage.tabStartPos + (i * tabMod) + new Vector3(cameraSize, 0, 0), Color.black);
			i++;
		}
		i = 0;
	}

	void printTab(float[,] tab, int x, int y)
	{
		int i= 0;
		int j = 0;

		while (i < x)
		{
			j = 0;
			while (j < y)
			{
				if (tab[i, j]  == 1f)
				{
					Vector3 start = new Vector3(DataStorage.tabStartPos.x + (i * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + (j * DataStorage.tabVSize));
					Vector3 end = new Vector3(DataStorage.tabStartPos.x + ((i + 1) * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + ((j + 1) * DataStorage.tabVSize));
					Vector3 start2 = new Vector3(DataStorage.tabStartPos.x + ((i + 1) * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + (j * DataStorage.tabVSize));
					Vector3 end2 = new Vector3(DataStorage.tabStartPos.x + (i * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + ((j + 1) * DataStorage.tabVSize));
					Debug.DrawLine(start, end, Color.blue, 0.1f);
					Debug.DrawLine(start2, end2, Color.blue, 0.1f);
					//Debug.Log("tab[" + i + "][" + j + "] = " + tab[i, j] );
				}
				else if (tab[i, j] == 2f)
				{
					Vector3 start = new Vector3(DataStorage.tabStartPos.x + (i * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + (j * DataStorage.tabVSize));
					Vector3 end = new Vector3(DataStorage.tabStartPos.x + ((i + 1) * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + ((j + 1) * DataStorage.tabVSize));
					Vector3 start2 = new Vector3(DataStorage.tabStartPos.x + ((i + 1) * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + (j * DataStorage.tabVSize));
					Vector3 end2 = new Vector3(DataStorage.tabStartPos.x + (i * DataStorage.tabHSize) , 0, DataStorage.tabStartPos.z + ((j + 1) * DataStorage.tabVSize));
					Debug.DrawLine(start, end, Color.red, 0.1f);
					Debug.DrawLine(start2, end2, Color.red, 0.1f);	
				}
				//Debug.Log(tab[i, j]);
				j++;
			}
			i++;
		}
	}
	float timePassed = 0f;
	void Update () {
		timePassed += Time.deltaTime;
		if (DataStorage.debug == 1)
		{
		//	ShowGrid();
			if (timePassed > 0.02f)
			{
				timePassed = 0f;
				printTab(DataStorage.botTab, (int)DataStorage.tabHNumber, (int)DataStorage.tabVNumber);
			}
		}
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
