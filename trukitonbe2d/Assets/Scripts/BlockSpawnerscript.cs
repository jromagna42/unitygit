using UnityEngine;
using System.Collections;

public class BlockSpawnerscript : MonoBehaviour {

	public GameObject fallingBlock;
	// Use this for initialization		
	float screensize;
	public float spawnTimer = 0.5f;
	int dif;
	void Start () {
		
		screensize = Camera.main.aspect * Camera.main.orthographicSize;
		dif = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		spawnTimer -= Time.deltaTime;
		if (spawnTimer < 0)
		{
			Vector2 spawnPos = new Vector2(Random.Range(-screensize, screensize), transform.position.y);
			Instantiate(fallingBlock, spawnPos, Quaternion.Euler(spawnPos));
			spawnTimer = 0.5f - (dif * 0.003f);
			if (dif < 200)
				dif++;
		}
	}
}
