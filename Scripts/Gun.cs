using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public Rigidbody2D bullet;				// Prefab of the bullet.
	private const float SPEED = 20f;		// The SPEED the bullet will fire at.
	public const float SHIFTX = 1.25f;   	// Constant x shift.
	public const float SHIFTY = 0.81f;   	// Constant y shift.
	private Vector3 position;				// For setting the position relative to the player.
	public bool allowedToShoot = true;		// Makes sure that the deltatime between the last shot is not too short.
	
	private Transform theTransform;			// Reference to the Transform.
	public AudioClip shootClip;				// Clip for when the player shoots.
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Transform component of Player.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		theTransform = transform;
		anim = transform.root.gameObject.GetComponent<Animator>();
		player = GameObject.FindWithTag("Player").transform;
		playerCtrl = transform.root.GetComponent<PlayerControl>();
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
	}

	private void FixedUpdate () {
		// Only able to shoot if the right input is pressed, the allowedToShoot boolean is true and the player is not a ghost.
		if (allowedToShoot && !playerCtrl.isGhost && Input.GetButtonDown("Shoot") || TouchShoot()) {
			// Set the animator Shoot trigger parameter and play the audioclip.
			allowedToShoot = false;
			if (playerCtrl.isRight)
				anim.SetTrigger("RightShoot");
			else
				anim.SetTrigger("LeftShoot");
			custom.PlayClipAt(shootClip, theTransform.position);
			// Derive the bullet's position from the player's position.
			if (playerCtrl.isRight) {
				position = new Vector3(player.position.x + SHIFTX, player.position.y + SHIFTY, 0);
				Rigidbody2D bulletInstance = Instantiate(bullet, position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(SPEED, 0);
			}
			else {
				position = new Vector3(player.position.x - SHIFTX, player.position.y + SHIFTY, 0);
				Rigidbody2D bulletInstance = Instantiate(bullet, position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-SPEED, 0);
			}
			StartCoroutine(Wait());
		}
	}

	private bool TouchShoot () {
		return (Input.touchCount == 1 && Input.touches[0].position.x > Screen.width/2 && Input.touches[0].position.y < Screen.height/2);
	}

	// You just shot the gun, wait to shoot again.
	private IEnumerator Wait () {
        yield return new WaitForSeconds(1f);
        allowedToShoot = true;
    }
}
