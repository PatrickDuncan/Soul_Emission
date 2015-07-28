using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class PlayerHealth : MonoBehaviour {
	public readonly float HEALTH = 10f;		// The player's health maximum.
	[HideInInspector]			
	public float currentH;					// The player's current health.
	[HideInInspector]
	public bool isDead = false;				// If the player is dead
	private object[,] reset;

	private Animator ripAnim;				// Reference to Rip's Animator
	private SpriteRenderer ripSprite;		// Reference to Rip's Sprite Renderer
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator on the player
	public Gun gun;							// Reference to the Gun class

	public AudioClip injuryClip;			// Clip for when the player gets injured.
	public AudioClip deathClip;				// Clip for when the player dies

	private void Awake () {
		ripAnim = GameObject.FindGameObjectWithTag("Rip").GetComponent<Animator>();
		ripSprite = GameObject.FindGameObjectWithTag("Rip").GetComponent<SpriteRenderer>();
		playerCtrl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
		gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
		currentH = HEALTH;
		loadReset();
	}

	private void loadReset () {
	    try {
	        string positions = Application.dataPath + "/Misc/Positions.txt";
	        int lines = File.ReadAllLines(positions).Length;
	        reset = new object[lines, 2];
	        StreamReader fileData = new StreamReader(positions, Encoding.Default);
	        for (int i=0; i<lines; i++) {
	        	string[] s = fileData.ReadLine().Split('$');
	        	string[] values = s[0].Split(' ');
	        	reset[i, 0] = new Vector3(Convert.ToSingle(values[0]), Convert.ToSingle(values[1]), Convert.ToSingle(values[2]));
	        	reset[i, 1] = Convert.ToBoolean(s[1]);
	        }
	        fileData.Close();
    	}
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }
		// reset = new object[,] { {new Vector3(-36.12f, -7.06f, 0f), true}, 	//Player
		// 					{new Vector3(0.67f, -6.91f, 0.58f), false} };	//Pointy Legs 1
	}

	public void TakeDamage (float damageAmount) {
		if (!playerCtrl.isGhost && !isDead) {
			currentH -= damageAmount;
			playerCtrl.helmetLight.intensity -= 0.2f;
			GetComponent<Rigidbody2D>().AddForce(new Vector2(-10f, 0), ForceMode2D.Impulse);
			if (currentH <= 0f) {
				isDead = true;
				if (playerCtrl.isRight)
					anim.SetTrigger("DeathRight");
				else
					anim.SetTrigger("DeathLeft");
				AudioSource.PlayClipAtPoint(deathClip, transform.position);
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
				playerCtrl.helmet.rotation = Quaternion.Euler(20f, 0f, 0f);
				gun.allowedToShoot = false;
				playerCtrl.allowedToGhost = false;
				ripSprite.enabled = true;
				ripAnim.enabled = true;
				StartCoroutine(Revive());
			} else
				AudioSource.PlayClipAtPoint(injuryClip, transform.position); 	//Only one sound when you die
		}
	}

	private IEnumerator Revive () {
    	yield return new WaitForSeconds(5f);
    	anim.SetTrigger("IdleRight");
    	GetComponent<PolygonCollider2D>().enabled = true;
    	currentH = HEALTH/2;
    	GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    	gun.allowedToShoot = true;
    	playerCtrl.allowedToGhost = true;
    	ripSprite.enabled = false;
    	ripAnim.enabled = false;
    	ResetPositions();
    	isDead = false;
	}

	//Reset the scene and bring the players HEALTH to half of the maximum
	private void ResetPositions () {
    	transform.position = (Vector3)reset[0, 0];
    	transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    	playerCtrl.isRight = (bool)reset[0, 1];
    	playerCtrl.resetHelmet();
    	playerCtrl.helmetLight.intensity = 1.5f;
    	GameObject.FindGameObjectWithTag("PointyLegs1").transform.position = (Vector3)reset[1, 0];
    	GameObject.FindGameObjectWithTag("PointyLegs1").GetComponent<PointyLegs>().isRight = (bool)reset[1, 1];
    	GameObject.FindGameObjectWithTag("PointyLegs1").GetComponent<PointyLegs>().health = 45f;
    }
}
