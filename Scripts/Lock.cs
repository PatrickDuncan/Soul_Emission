using UnityEngine;

public class Lock : MonoBehaviour {
	
	private readonly Vector3 SHIFT = new Vector3(0f, 10.68f, 1f);	// Constant shift to be in the proper position relative to the player.

	private Transform theTransform;			// Reference to the Transform.
	private Transform player;				// Reference to the player.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.

	private void Awake () {
		theTransform = transform;
		GameObject gO  = GameObject.FindWithTag("Player");
		playerH = gO.GetComponent<PlayerHealth>();
		player = gO.transform;
	}

	private void Update () {
		// Set the position to the player's position with the offset.
		if (!playerH.isDead) { 
			theTransform.position = player.position - SHIFT;	
		}
	}
}
