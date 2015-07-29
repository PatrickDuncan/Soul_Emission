using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public Rigidbody2D bullet;				// Prefab of the bullet.
	private readonly float SPEED = 20f;		// The SPEED the bullet will fire at.
	public readonly float SHIFTX = 1.25f;   // Constant x shift.
	public readonly float SHIFTY = 0.81f;   // Constant y shift.
	private Vector3 position;				// For setting the position relative to the player.
	public bool allowedToShoot = true;		// Makes sure that the deltatime between the last shot is not too short.
	
	public AudioClip shootClip;				// Clip for when the player shoots.
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Transform component of Player.

	private void Awake () {
		anim = transform.root.gameObject.GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerCtrl = transform.root.GetComponent<PlayerControl>();
	}

	private void FixedUpdate () {
		//Only able to shoot if the right input is pressed, the allowedToShoot boolean is true and the player is not a ghost.
		if ((Input.GetButtonDown("Fire1") || (Input.touchCount == 1 && Input.touches[0].position.x > Screen.width/2 && Input.touches[0].position.y < Screen.height/2)) && allowedToShoot && !playerCtrl.isGhost) {
			// ... set the animator Shoot trigger parameter and play the audioclip.
			allowedToShoot = false;
        	playerCtrl.allowedToGhost = false;
			if (playerCtrl.isRight)
				anim.SetTrigger("RightShoot");
			else
				anim.SetTrigger("LeftShoot");
			AudioSource.PlayClipAtPoint(shootClip, transform.position);
			if (playerCtrl.isRight) {
				position = new Vector3(player.position.x + SHIFTX, player.position.y + SHIFTY, 0);
				Rigidbody2D bulletInstance = Instantiate(bullet, position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(SPEED, 0);
			}
			else {
				position = new Vector3(player.position.x - SHIFTX, player.position.y + SHIFTY, 0);
				Rigidbody2D bulletInstance = Instantiate(bullet, position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-SPEED, 0);
			}
			StartCoroutine(Wait());
		}
	}

	//You just shot the gun, wait to shoot again.
	private IEnumerator Wait () {
        yield return new WaitForSeconds(1.2f);
        allowedToShoot = true;
        playerCtrl.allowedToGhost = true;
    }
}
