using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {

	public event System.Action OnPlayerDeath;
	public float speed = 10; 
	float	screensize;
		// Use this for initialization
	void Start () {
		float halfplayer = transform.localScale.x / 2f;
		screensize = Camera.main.aspect * Camera.main.orthographicSize + halfplayer;

	}
	
	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxisRaw("Horizontal");
		float velocity = inputX * speed;
		transform.Translate (Vector2.right * velocity * Time.deltaTime);

		if (transform.position.x < -screensize)
		{
			transform.position = new Vector2(screensize, transform.position.y);
		}
		if (transform.position.x > screensize)
		{
			transform.position = new Vector2(-screensize, transform.position.y);
		}

	}
	void OnTriggerEnter2D(Collider2D triggerOn)
	{
		if (triggerOn.tag == "fallingblock")
			{
				if (OnPlayerDeath != null)
				{
					OnPlayerDeath();
				}
				Destroy(gameObject);
			}
	}
	
}
