using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public Rigidbody2D bullet0;				// Prefab of the bullet0.
	public Rigidbody2D bullet1;				// Prefab of the bullet1.
	public Rigidbody2D bullet2;				// Prefab of the bullet2.
	public enum bullets {SMG, Pistol, Sniper};		// The only ammo types.
	private const float SPEED = 20f;		// The SPEED the bullet will fire at.
	public const float SHIFTX = 1.25f;   	// Constant x shift.
	public const float SHIFTY = 0.81f;   	// Constant y shift.
	public int bulletType;					// Which bullet type to use.
	public float[] waitTimes = {0.5f, 1f, 2f};		// The wait times for the various ammunitions.
	public float[] dmgAmounts = {3f, 7f, 20f};	// The damage amounts for the various ammunitions.
	private Vector3 position;				// For setting the position relative to the player.
	
	private Transform theTransform;			// Reference to the Transform.
	public AudioClip SMGClip;				// Clip for when the player shoots with SMG.
	public AudioClip pistolClip;			// Clip for when the player shoots with Pistol.
	public AudioClip sniperClip;			// Clip for when the player shoots with Sniper.
	private PlayerControl playerCtrl;		// Reference to the Player Control script.
	private PlayerHealth playerH;			// Reference to the Player Health script.
	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Transform component of Player.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		theTransform = transform;
		anim = transform.root.gameObject.GetComponent<Animator>();
		player = GameObject.FindWithTag("Player").transform;
		playerCtrl = transform.root.GetComponent<PlayerControl>();
		playerH = transform.root.GetComponent<PlayerHealth>();
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
		bulletType = (int)bullets.Pistol;
	}

	private void Update () {
		if (Application.loadedLevel != 0) {
			if (!playerH.isDead && playerCtrl.allowedToShoot && !playerCtrl.isGhost && CustomGetButtonDown.ButtonDown() || TouchShoot()) {
				playerCtrl.allowedToShoot = false;
				if (playerCtrl.isRight)
					anim.SetTrigger("RightShoot");
				else
					anim.SetTrigger("LeftShoot");
				// Derive the bullet's position from the player's position.
				Rigidbody2D bulletR;
				if (bulletType == 0) { 
					bulletR = bullet0;
					custom.PlayClipAt(SMGClip, theTransform.position);
				}
				else if (bulletType == 1) {
					bulletR = bullet1;
					custom.PlayClipAt(pistolClip, theTransform.position);
				}
				 else if (bulletType == 2) {
					bulletR = bullet2;
					custom.PlayClipAt(sniperClip, theTransform.position);
				} 
				else
					bulletR = bullet2;
				if (playerCtrl.isRight) {
					position = new Vector3(player.position.x + SHIFTX, player.position.y + SHIFTY, 0);
					Rigidbody2D bulletInstance = Instantiate(bulletR, position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(SPEED, 0);
				}
				else {
					position = new Vector3(player.position.x - SHIFTX, player.position.y + SHIFTY, 0);
					Rigidbody2D bulletInstance = Instantiate(bulletR, position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(-SPEED, 0);
				}
				StartCoroutine(Wait());
			}
			CustomGetButtonDown.ButtonUp();
		}
	}

	private bool TouchShoot () {
		return (Input.touchCount == 1 && Input.touches[0].position.x > Screen.width/2 && Input.touches[0].position.y < Screen.height/2);
	}

	// You just shot the gun, wait to shoot again.
	private IEnumerator Wait () {
        yield return new WaitForSeconds(waitTimes[bulletType]);
     	playerCtrl.allowedToShoot = true;
    }
}
