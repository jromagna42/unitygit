using UnityEngine;
using System.Collections;

public class wall : MonoBehaviour {

	public Sprite dmgsprite;
	public int hp = 4;

	private SpriteRenderer spriteRenderer;

/// <summary>
/// Awake is called when the script instance is being loaded.
/// </summary>
void Awake()
{
	spriteRenderer = GetComponent<SpriteRenderer>();
}

	public void damagewall(int loss)
	{
		spriteRenderer.sprite = dmgsprite;
		hp -= loss;
		if (hp <= 0)
			gameObject.SetActive(false);
	}	
}
