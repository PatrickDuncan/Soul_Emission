using UnityEngine;

public class Lock : MonoBehaviour {
	
	private Transform theTransform;			// Reference to the Transform.
	private Transform player;				// Reference to the player.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.

	private void Awake () {
		theTransform = transform;
		GameObject gO = GameObject.FindWithTag("Player");
		playerH = gO.GetComponent<PlayerHealth>();
		player = gO.transform;
	}

	private void Update () {
		
		Vector3 SHIFT;		// Constant shift to be in the proper position relative to the player.
		if (!playerH.isDead)
			SHIFT = new Vector3(0f, 10.68f, 1f);
		else 
			SHIFT = new Vector3(0f, 8f, 1f);
		// Set the position to the player's position with the offset.
		theTransform.position = player.position - SHIFT;	
	}
}
