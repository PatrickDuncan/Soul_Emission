using UnityEngine;

public class Scenes : MonoBehaviour {

	private Vector3[] playerPos = new Vector3[6];
	private Vector3[] pointyPos = new Vector3[18];
	private Vector3[] fourEyesPos = new Vector3[5];
	private Vector3[] explodetaurPos = new Vector3[5];
	private Vector3[] pointyFlip = new Vector3[18];
	private Vector3[] fourEyesFlip = new Vector3[5];
	private Vector3[] explodetaurFlip = new Vector3[5];

	private Quaternion[] pointyRot = new Quaternion[18];
	private Quaternion[] fourEyesRot = new Quaternion[5];
	private Quaternion[] explodetaurRot = new Quaternion[5];

	private bool[] pointyAlive = new bool[18];
	private bool[] fourEyesAlive = new bool[5];
	private bool[] explodetaurAlive = new bool[5];
	private bool[] usedHealths = new bool[6];

	private int[] pointyStart = {0, 1, 6, 12, 0};
	private int[] fourEyesStart = {0, 0, 0, 0, 3};
	private int[] explodetaurStart = {0, 0, 0, 0, 0};
	private int level;

	private Positions positions;			// Reference to the Positions class.

	private void Awake () {
		positions = GameObject.FindWithTag("Scripts").GetComponent<Positions>();
		level = Application.loadedLevel;
	}

	private void OnLevelWasLoaded (int level) {
		this.level = level;
	}

	public void Save (GameObject[] enemies) {
		usedHealths[level] = GameObject.FindWithTag("Health").GetComponent<HealthPickup>().used;
		playerPos[level] = GameObject.FindWithTag("Player").transform.position;
		int j = pointyStart[level];
		for (int i=0; i<enemies.Length; i++) {
	    	if (enemies[i].name.Contains("Pointy Legs")) {
		    	pointyPos[j] = enemies[i].transform.position;
		    	pointyRot[j] = enemies[i].transform.rotation;
		    	pointyFlip[j] = enemies[i].transform.localScale;
		    	if (enemies[i].GetComponent<PointyLegs>().health > 0f) {
		    		pointyAlive[j] = true;
		    	}
		    	else
		    		pointyAlive[j] = false;
		    	j++;
		    	if (j == positions.pointy.Length)
		    		break;
		    }
	    }
	    j = fourEyesStart[level];
	    for (int i=0; i<enemies.Length; i++) {
	    	if (enemies[i].name.Contains("Four Eyes")) {
		    	fourEyesPos[j] = enemies[i].transform.position;
		    	fourEyesRot[j] = enemies[i].transform.rotation;
		    	fourEyesFlip[j] = enemies[i].transform.localScale;
		    	if (enemies[i].GetComponent<FourEyes>().health > 0f) {
		    		fourEyesAlive[j] = true;
		    	}
		    	else
		    		fourEyesAlive[j] = false;
		    	j++;
		    	if (j == positions.fourEyes.Length)
		    		break;
		    }
	    }
	    j = explodetaurStart[level];
	    for (int i=0; i<enemies.Length; i++) {
	    	if (enemies[i].name.Contains("Explodetaur")) {
		    	explodetaurPos[j] = enemies[i].transform.position;
		    	explodetaurPos[j] = enemies[i].transform.position;
		    	explodetaurFlip[j] = enemies[i].transform.localScale;
		    	if (enemies[i].GetComponent<Explodetaur>().health > 0f) {
		    		explodetaurAlive[j] = true;
		    	}
		    	else
		    		explodetaurAlive[j] = false;
		    	j++;
		    	if (j == positions.explodetaur.Length)
		    		break;
		    }
	    }
	}

	public void Load (GameObject[] enemies)	{
		if (usedHealths[level]) {
			GameObject.FindWithTag("Health").GetComponent<HealthPickup>().SpriteChange();
		}
		GameObject.FindWithTag("Player").transform.position = playerPos[level];
		int j = pointyStart[level];
		for (int i=0; i<enemies.Length; i++) {
	    	if (enemies[i].name.Contains("Pointy Legs")) {
		    	enemies[i].transform.position = pointyPos[j];
		    	enemies[i].transform.localScale = pointyFlip[j];
		    	enemies[i].transform.rotation = pointyRot[j];
		    	if (pointyAlive[j]) {
		    		enemies[i].GetComponent<PointyLegs>().health = 45f;
		    	}
		    	else
		    		enemies[i].GetComponent<PointyLegs>().DeathState();
		    	j++;
		    	if (j == positions.pointy.Length)
		    		break;
		    }
	    }
	    j = fourEyesStart[level];
	    for (int i=0; i<enemies.Length; i++) {
	    	if (enemies[i].name.Contains("Four Eyes")) {
		    	enemies[i].transform.position = fourEyesPos[j];
		    	enemies[i].transform.localScale = fourEyesFlip[j];
		    	enemies[i].transform.rotation =	fourEyesRot[j];
		    	if (fourEyesAlive[j]) {
		    		enemies[i].GetComponent<FourEyes>().health = 100f;
		    	}
		    	else
		    		enemies[i].GetComponent<FourEyes>().DeathState();
		    	j++;
		    	if (j == positions.fourEyes.Length)
		    		break;
		    }
	    }
	    j = explodetaurStart[level];
	    for (int i=0; i<enemies.Length; i++) {
	    	if (enemies[i].name.Contains("Explodetaur")) {
		    	enemies[i].transform.position = explodetaurPos[j];
		    	enemies[i].transform.localScale = explodetaurFlip[j];
		    	enemies[i].transform.rotation = explodetaurRot[j];
		    	if (explodetaurAlive[j]) {
		    		enemies[i].GetComponent<Explodetaur>().health = 14f;
		    	}
		    	else
		    		enemies[i].GetComponent<Explodetaur>().DeathState();
		    	j++;
		    	if (j == positions.explodetaur.Length)
		    		break;
		    }
	    }	
	}
}