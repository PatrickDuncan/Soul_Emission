using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {
	
	private Vector3 shit = new Vector3(0f, 25.08f, 0f);
	private bool h;
	private Vector3 prePosition;		//Position of lower block before death
	private Quaternion preRotation;

	private Transform player;			// Reference to the player.
	private PlayerHealth playerH;		// Reference to the PlayerHealth script.

	void Awake () {
		playerH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		prePosition = transform.position;
		preRotation = transform.rotation;
	}

	void Update () {
		// Set the position to the player's position with the offset.
		if (!playerH.isDead) { 
			prePosition = transform.position;
			transform.position = player.position - shit;
			
		}
		// else {
		// 	preRotation = transform.rotation;
		// 	prePosition = transform.position;
		// 	h = false;
		// }
	}
}
