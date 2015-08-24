using UnityEngine;
using System;

public class Reset : MonoBehaviour {

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Transform playerTran;			// Referefence to the Player's transform.
	public Transform helmet;				// Reference to the helmet object's transfrom.
	public Light helmetLight;				// Reference to the helmet object's light. Initialized in Unity.
	private Animator anim;					// Reference to the Animator component. Initialized in Unity.				
	private Positions positions;			// Reference to the Positions class.

	private GameObject player;				// Reference to the player's game object.
	public AudioClip kissClip;				// Normal background song.
	public AudioClip expanseClip;			// Secondary background song.
	public Quaternion defaultLight;			// Default position of the helmet light


	private void Awake () {
		player = GameObject.FindWithTag("Player");
		playerCtrl = player.GetComponent<PlayerControl>();
		playerTran = player.transform;
		anim = player.GetComponent<Animator>();
		positions = GameObject.FindWithTag("Scripts").GetComponent<Positions>();
		defaultLight = Quaternion.Euler(16f, 106f, 220f);
	}

	private void Start () {
		ResetPosition();
	}
	
	private void OnLevelWasLoaded (int level) {
        ResetPosition();
        AudioSource audio = player.GetComponent<AudioSource>();
        if (level == 4)
        	audio.clip = expanseClip;
        else
        	audio.clip = kissClip;
        audio.Play();
    }

    // Reset the scene and bring the players HEALTH to half of the maximum
	public void ResetScene () {
		try {
	    	playerTran.rotation = Quaternion.Euler(0f, 0f, 0f);
	    	helmetLight.intensity = 1.575f;
	    	ResetPosition();
	    	ResetHelmet();
	    	// Loop through all the pointy legs in the scene and reset their positions.
	    	int j = 0;
	    	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
	    	for (int i=0; i<enemies.Length; i++) {
		    	if (enemies[i].name.Contains("Pointy Legs")) {
			    	enemies[i].transform.position = positions.pointy[i];
			    	if (enemies[i].GetComponent<PointyLegs>().health > 0f)
			    		enemies[i].GetComponent<PointyLegs>().health = 45f;
			    	j++;
			    	if (j == positions.pointy.Length)
			    		break;
			    }
		    }
		  //  j = positions.fourEyesStart;
		    j = 0;
		    for (int i=0; i<positions.fourEyes.Length; i++) {
		    	if (enemies[i].name.Contains("Four Eyes")) {
			    	enemies[i].transform.position = positions.fourEyes[i];
			    	if (enemies[i].GetComponent<FourEyes>().health > 0f)
			    		enemies[i].GetComponent<FourEyes>().health = 45f;
			    	j++;
			    	if (j == positions.fourEyes.Length)
			    		break;
			    }
		    }
		} catch (Exception e) {
			print(e);
		}
    }

    public void ResetPosition () {
    	playerTran.position = positions.player;
    	playerCtrl.isRight = positions.isRight;
    }

	public void ResetHelmet () {
		helmet.rotation = defaultLight;
		if (playerCtrl.isRight) {
			if (playerCtrl.isGhost)
				anim.SetTrigger("GhostRight");
			else 
				anim.SetTrigger("IdleRight");
			helmet.position = new Vector3(-1.54f + playerTran.position.x, 2.36f + playerTran.position.y, -0.42f);
		}
		else {
			if (playerCtrl.isGhost)
				anim.SetTrigger("GhostLeft");
			else 
				anim.SetTrigger("IdleLeft");
			helmet.RotateAround(Vector3.zero, Vector3.up, 148f);
			helmet.position = new Vector3(1.54f + playerTran.position.x, 2.36f + playerTran.position.y, -0.42f);
		}
	}
}