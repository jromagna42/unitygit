using UnityEngine;
using System.Collections;

public class player : MovingObject
{

	public int walldamage = 1;
	public int pointperfood = 10;
	public int pointpersoda = 20;
	public float restartleveldelay = 1f;

	private Animator animator;
	private int food;

	// Use this for initialization
	protected override void Start()
	{
		animator = GetComponent<Animator>();
		food = GameManager.instance.playerfoodpoint;

		base.Start();
	}

	private void ondisable()
	{
		GameManager.instance.playerfoodpoint = food;
	}
	// Update is called once per frame
	void Update()
	{
		 if(!GameManager.instance.playerturn) return;
		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw("Horizontal");
		vertical = (int)Input.GetAxisRaw("Vertical");

		if (horizontal != 0)
			vertical = 0;
		if (horizontal != 0 || vertical != 0)
			attemptmove<wall>(horizontal, vertical);

	}

	protected override void attemptmove<T>(int xdir, int ydir)
	{
		food--;
		base.attemptmove<T>(xdir, ydir);
		RaycastHit2D hit;
		checkifgameover();
		GameManager.instance.playerturn = false;
	}

	protected override void oncantmove<T>(T component)
	{
		wall hitwall = component as wall;
		hitwall.damagewall(walldamage);
		animator.ResetTrigger("playerchop");

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		print("contact");
		print(food);
		if (other.tag == "Exit")
		{
			Invoke("restart", restartleveldelay);
			enabled = false;
		}
		else if (other.tag == "Food")
		{
			food += pointperfood;
			other.gameObject.SetActive(false);
		}
		else if (other.tag == "Soda")
		{
			food += pointpersoda;
			other.gameObject.SetActive(false);
		}
	}

	private void restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void lodefood(int loss)
	{
		animator.SetTrigger("playerhit");
		food -= loss;
		checkifgameover();
	}
	private void checkifgameover()
	{
		if (food <= 0)
			GameManager.instance.gameover();
	}
}
