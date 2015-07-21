using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public Rigidbody2D bullet;				// Prefab of the bullet.
	public float speed = 15f;				// The speed the bullet will fire at.
	public float shiftX = 1.25f;
	public float  shiftY = 0.81f;
	private Vector3 position;
	public bool allowedToShoot = true;		//Makes sure that the deltatime between the last shot is not too short.
	public AudioClip shootClip;			// Clip for when the player shoots.

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	private Transform player;

	private void Awake() {
		anim = transform.root.gameObject.GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerCtrl = transform.root.GetComponent<PlayerControl>();
	}

	private void FixedUpdate () {
		if (Input.GetButtonDown("Fire1") && allowedToShoot) {
			// ... set the animator Shoot trigger parameter and play the audioclip.
			if (playerCtrl.isRight)
				anim.SetTrigger("RightShoot");
			else
				anim.SetTrigger("LeftShoot");
			AudioSource.PlayClipAtPoint(shootClip, transform.position);
			if (playerCtrl.isRight) {
				position = new Vector3(player.position.x + shiftX, player.position.y + shiftY, 0);
				Rigidbody2D bulletInstance = Instantiate(bullet, position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else {
				position = new Vector3(player.position.x - shiftX, player.position.y + shiftY, 0);
				Rigidbody2D bulletInstance = Instantiate(bullet, position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
			StartCoroutine(Wait());
		}
	}

	//You just shot the gun, wait to shoot again.
	private IEnumerator Wait() {
        allowedToShoot = false;
        yield return new WaitForSeconds(0.8f);
        allowedToShoot = true;
    }
}
