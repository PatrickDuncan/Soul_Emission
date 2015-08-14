using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {
	
	private readonly Vector3 SHIFT = new Vector3(0f, 25.08f, 0f);	// Constant shift to be in the proper position relative to the player.

	private Transform player;			// Reference to the player.
	private PlayerHealth playerH;		// Reference to the PlayerHealth script.

	void Awake () {
		DontDestroyOnLoad(transform.gameObject);
		playerH = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
		player = GameObject.FindWithTag("Player").transform;
	}

	void Update () {
		// Set the position to the player's position with the offset.
		if (!playerH.isDead) { 
			transform.position = player.position - SHIFT;	
		}
	}
}
