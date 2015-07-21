using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	[HideInInspector]	
	public float health = 50f;					// The player's health maximum.
	[HideInInspector]			
	public float currentH;						// The player's current health.
	[HideInInspector]
	public bool isDead = false;					// If the player is dead

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player

	private object[,] reset;

	private void Awake () {
		playerCtrl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
		currentH = health;
		loadReset();
	}

	private void loadReset () {
		reset = new object[,] { {new Vector3(-36.12f, -7.06f, 0f), true}, 	//Player
							{new Vector3(0.67f, -6.91f, 0.58f), false} };	//Pointy Legs 1
	}

	public void TakeDamage (float damageAmount) {
		currentH -= damageAmount;
		if (currentH <= 0f && !isDead) {
			isDead = true;
			anim.SetTrigger("DeathRight");
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			playerCtrl.helmet.rotation = Quaternion.Euler(20f, 0f, 0f);
			StartCoroutine(Revive());
		}
	}

	private IEnumerator Revive () {
    	yield return new WaitForSeconds(3f);
    	anim.SetTrigger("IdleRight");
    	GetComponent<PolygonCollider2D>().enabled = true;
    	currentH = health/2;
    	GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    	ResetPositions();
    	isDead = true;
	}

	//Reset the scene and bring the players health to half of the maximum
	private void ResetPositions () {
    	transform.position = (Vector3)reset[0, 0];
    	transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    	playerCtrl.isRight = (bool)reset[0, 1];;
    	playerCtrl.resetHelmet();
    	GameObject.FindGameObjectWithTag("PointyLegs1").transform.position = (Vector3)reset[1, 0];
    	GameObject.FindGameObjectWithTag("PointyLegs1").GetComponent<PointyLegs>().isRight = (bool)reset[1, 1];
    	GameObject.FindGameObjectWithTag("PointyLegs1").GetComponent<PointyLegs>().health = 45f;
    }
}
