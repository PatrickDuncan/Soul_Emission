using UnityEngine;
using System;

public class Scenes : MonoBehaviour {

	private Vector3[] playerPos = new Vector3[6];				// Array of saved player positions to load.
	private Vector3[] pointyPos = new Vector3[18];				// Array of saved pointy legs positions to load.
	private Vector3[] fourEyesPos = new Vector3[5];				// Array of saved four eyes positions to load.
	private Vector3[] explodetaurPos = new Vector3[5];			// Array of saved explodetaur positions to load.
	private Vector3[] pointyFlip = new Vector3[18];				// Array of saved pointy legs scales to load.
	private Vector3[] fourEyesFlip = new Vector3[5];			// Array of saved four eyes scales to load.
	private Vector3[] explodetaurFlip = new Vector3[5];			// Array of saved explodetaur scales to load.

	private Quaternion[] pointyRot = new Quaternion[18];		// Array of saved pointy legs rotations to load.
	private Quaternion[] fourEyesRot = new Quaternion[5];		// Array of saved four eyes rotations to load.
	private Quaternion[] explodetaurRot = new Quaternion[5];	// Array of saved explodetaur rotations to load.

	private bool[] pointyAlive = new bool[18];					// Array of saved pointy legs mortality to load.
	private bool[] fourEyesAlive = new bool[5];					// Array of saved four eyes mortality to load.
	private bool[] explodetaurAlive = new bool[5];				// Array of saved explodetaur mortality to load.
	private bool[] usedHealths = new bool[6];					// Array of used healths to load.

	private int[] pointyStart = {0,0, 1, 6, 12, 0};				// The start index in every scene for pointy legs.
	private int[] fourEyesStart = {0, 0, 0, 0, 0, 3};				// The start index in every scene for four eyes.
	private int[] explodetaurStart = {0, 0, 0, 0, 0, 0};			// The start index in every scene for explodetaur.
	private int level;											// The current level/scene.

	private PlayerControl playerCtrl;

	private void Awake () {
		level = Application.loadedLevel;
		playerCtrl = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
	}

	private void OnLevelWasLoaded (int level) {
		this.level = level;
	}

	public void Save (GameObject[] enemies) {
		try {
			// Save used healths
			usedHealths[level] = GameObject.FindWithTag("Health").GetComponent<HealthPickup>().used;
			Vector3 player = GameObject.FindWithTag("Player").transform.position; 
			// Save the player's position with an offset so the player doesn't instantly collide with the door on load
			if (player.x < 0)
				playerPos[level] = player + new Vector3(1, 0, 0);
			else if (player.x > 0)
				playerPos[level] = player - new Vector3(1, 0, 0);
			int pointyIndex, fourEyesIndex, explodetaurIndex;
			pointyIndex = pointyStart[level];
			fourEyesIndex = fourEyesStart[level];
			explodetaurIndex = explodetaurStart[level];
			// Loop through all the enemies in the scene and save their position, rotation and scale
			// Record if they're dead as well.
			for (int i=0; i<enemies.Length; i++) {
		    	if (enemies[i].name.Contains("Pointy Legs")) {
			    	pointyPos[pointyIndex] = enemies[i].transform.position;
			    	pointyRot[pointyIndex] = enemies[i].transform.rotation;
			    	pointyFlip[pointyIndex] = enemies[i].transform.localScale;
			    	if (enemies[i].GetComponent<PointyLegs>().health > 0f)
			    		pointyAlive[pointyIndex] = true;
			    	else
			    		pointyAlive[pointyIndex] = false;
			    	pointyIndex++;
			    }
			    else if (enemies[i].name.Contains("Four Eyes")) {
			    	fourEyesPos[fourEyesIndex] = enemies[i].transform.position;
			    	fourEyesRot[fourEyesIndex] = enemies[i].transform.rotation;
			    	fourEyesFlip[fourEyesIndex] = enemies[i].transform.localScale;
			    	if (enemies[i].GetComponent<FourEyes>().health > 0f)
			    		fourEyesAlive[fourEyesIndex] = true;
			    	else
			    		fourEyesAlive[fourEyesIndex] = false;
			    	fourEyesIndex++;
			    } 
			    else if (enemies[i].name.Contains("Explodetaur")) {
			    	explodetaurPos[explodetaurIndex] = enemies[i].transform.position;
			    	explodetaurRot[explodetaurIndex] = enemies[i].transform.rotation;
			    	explodetaurFlip[explodetaurIndex] = enemies[i].transform.localScale;
			    	if (enemies[i].GetComponent<Explodetaur>().health > 0f)
			    		explodetaurAlive[explodetaurIndex] = true;
			    	else
			    		explodetaurAlive[explodetaurIndex] = false;
			    	explodetaurIndex++;
			    }
		    }
		}
		catch (Exception e) {
			print(e);
		}
	}

	public void Load (GameObject[] enemies)	{
		// If the player has completed that level, turn on the door light they already  
		// had to turn on early to advance 
		if (playerCtrl.completed[level])
			GameObject.FindWithTag("Exit").GetComponentInChildren<Light>().enabled = true;
		// Set used healths to the used state
		if (usedHealths[level])
			GameObject.FindWithTag("Health").GetComponent<HealthPickup>().SpriteChange();
		GameObject.FindWithTag("Player").transform.position = playerPos[level];
		int pointyIndex, fourEyesIndex, explodetaurIndex;
		pointyIndex = pointyStart[level];
		fourEyesIndex = fourEyesStart[level];
		explodetaurIndex = explodetaurStart[level];
		// Loop through all the enemies in the scene and set their position, rotation and scale
		// Set their health to maximum if they're alive or put them back to the death state.
		for (int i=0; i<enemies.Length; i++) {
	    	if (enemies[i].name.Contains("Pointy Legs")) {
		    	enemies[i].transform.position = pointyPos[pointyIndex];
		    	enemies[i].transform.rotation = pointyRot[pointyIndex];
		    	enemies[i].transform.localScale = pointyFlip[pointyIndex];
		    	if (pointyAlive[pointyIndex])
		    		enemies[i].GetComponent<PointyLegs>().health = 45f;
		    	else 
		    		enemies[i].GetComponent<PointyLegs>().DeathState();
		    	pointyIndex++;
		    }
		    else if (enemies[i].name.Contains("Four Eyes")) {
		    	enemies[i].transform.position = fourEyesPos[fourEyesIndex];
		    	enemies[i].transform.rotation = fourEyesRot[fourEyesIndex];
		    	enemies[i].transform.localScale = fourEyesFlip[fourEyesIndex];
		    	if (fourEyesAlive[fourEyesIndex])
		    		enemies[i].GetComponent<FourEyes>().health = 100f;
		    	else
		    		enemies[i].GetComponent<FourEyes>().DeathState();
		    	fourEyesIndex++;
		    } 
		    else if (enemies[i].name.Contains("Explodetaur")) {
		    	enemies[i].transform.position = explodetaurPos[explodetaurIndex];
		    	enemies[i].transform.rotation = explodetaurRot[explodetaurIndex];
		    	enemies[i].transform.localScale = explodetaurFlip[explodetaurIndex];
		    	if (explodetaurAlive[explodetaurIndex])
		    		enemies[i].GetComponent<Explodetaur>().health = 14f;
		    	else
		    		enemies[i].GetComponent<Explodetaur>().DeathState();
		    	explodetaurIndex++;
		    }
	    }
	}
}