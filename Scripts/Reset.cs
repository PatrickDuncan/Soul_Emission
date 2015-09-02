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
	public AudioClip driftingClip;				// Normal background song.
	public Quaternion defaultLight;			// Default position of the helmet light


	private void Awake () {
		player = GameObject.FindWithTag("Player");
		playerCtrl = player.GetComponent<PlayerControl>();
		playerTran = player.transform;
		anim = player.GetComponent<Animator>();
		positions = GameObject.FindWithTag("Scripts").GetComponent<Positions>();
		defaultLight = Quaternion.Euler(16f, 106f, 220f);
        player.GetComponent<AudioSource>().pitch = 1f;
	}

	private void Start () {
		ResetPosition();
	}
	
	private void OnLevelWasLoaded (int level) {
        AudioSource audio = player.GetComponent<AudioSource>();
        if (level%2 == 1) {
        	audio.clip = kissClip;
        	audio.pitch = 0.4f;
        }
        else if (level%2 == 0) {
        	audio.clip = expanseClip;
        	audio.pitch = 0.4f;
        }
        audio.Play();
    }

    // Reset the scene and bring the players HEALTH to half of the maximum
	public void ResetScene () {
		try {
	    	playerTran.rotation = Quaternion.Euler(0f, 0f, 0f);
	    	helmetLight.intensity = 3f;
	    	ResetPosition();
	    	ResetHelmet();
	    	// Loop through all the enemies in the scene and reset their positions.

		// 	int j = 0;
	 //    	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
	 //    	for (int i=0; i<enemies.Length; i++) {
		//     	if (enemies[i].name.Contains("Pointy Legs")) {
			    	
		// 	    	if (enemies[i].GetComponent<PointyLegs>().health > 0f) {
		// 	    		enemies[i].transform.position = positions.pointy[j];
		// 	    		enemies[i].GetComponent<PointyLegs>().health = 45f;
		// 	    	}
		// 	    	j++;
		// 	    	if (j == positions.pointy.Length)
		// 	    		break;
		// 	    }
		//     }
		// //  j = positions.fourEyesStart;
		//     j = 0;
		//     for (int i=0; i<enemies.Length; i++) {
		//     	if (enemies[i].name.Contains("Four Eyes")) {
			    	
		// 	    	if (enemies[i].GetComponent<FourEyes>().health > 0f) {
		// 	    		print(positions.fourEyes[j]);
		// 	    		enemies[i].transform.position = positions.fourEyes[j];
		// 	    		enemies[i].GetComponent<FourEyes>().health = 100f;
		// 	    	}
		// 	    	j++;
		// 	    	if (j == positions.fourEyes.Length)
		// 	    		break;
		// 	    }
		//     }
		//     j = 0;
		//     for (int i=0; i<enemies.Length; i++) {
		//     	if (enemies[i].name.Contains("Explodetaur")) {
			    	
		// 	    	if (enemies[i].GetComponent<Explodetaur>().health > 0f) {
		// 	    		enemies[i].transform.position = positions.explodetaur[j];
		// 	    		enemies[i].GetComponent<Explodetaur>().health = 14f;
		// 	    	}
		// 	    	j++;
		// 	    	if (j == positions.explodetaur.Length)
		// 	    		break;
		// 	    }
		//     }

	    	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		    int pointyIndex = 0, explodetaurIndex = 0;
		    for (int i=0; i<enemies.Length; i++) {
		    	if (enemies[i].name.Contains("Pointy Legs")) {
			    	if (enemies[i].GetComponent<PointyLegs>().health > 0f) {
			    		enemies[i].transform.position = positions.pointy[pointyIndex];
			    		enemies[i].GetComponent<PointyLegs>().health = 45f;
			    	}
			    	pointyIndex++;
			    }
			    else if (enemies[i].name.Contains("Explodetaur")) {
			    	if (enemies[i].GetComponent<Explodetaur>().health > 0f) {
			    		enemies[i].transform.position = positions.explodetaur[explodetaurIndex];
			    		enemies[i].GetComponent<Explodetaur>().health = 14f;
			    	}
			    	explodetaurIndex++;
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