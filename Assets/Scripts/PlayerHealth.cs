using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	[HideInInspector]	
	public float health = 50f;				// The player's health maximum.
	[HideInInspector]			
	public float currentH;					// The player's current health.
	[HideInInspector]
	public bool isDead = false;				// If the player is dead

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator on the player
	private Gun gun;						// Reference to the Gun class

	private object[,] reset;
	public AudioClip injuryClip;			// Clip for when the player gets injured.
	public AudioClip deathClip;				// Clip for when the player dies

	private void Awake () {
		playerCtrl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
		gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
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
			AudioSource.PlayClipAtPoint(deathClip, transform.position);
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			playerCtrl.helmet.rotation = Quaternion.Euler(20f, 0f, 0f);
			gun.allowedToShoot = false;
			StartCoroutine(Revive());
		} else
			AudioSource.PlayClipAtPoint(injuryClip, transform.position); 	//Only one sound when you die
	}

	private IEnumerator Revive () {
    	yield return new WaitForSeconds(3f);
    	anim.SetTrigger("IdleRight");
    	GetComponent<PolygonCollider2D>().enabled = true;
    	currentH = health/2;
    	GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    	gun.allowedToShoot = true;
    	ResetPositions();
    	isDead = false;
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
