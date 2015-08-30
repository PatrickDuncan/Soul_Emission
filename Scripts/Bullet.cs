using UnityEngine;

public class Bullet : MonoBehaviour {

	private Gun gun;						// Reference to the GUn class in Player.

	private void Start () {
		gun = GameObject.FindWithTag("Player").GetComponentInChildren<Gun>();
		Destroy(gameObject, 0.8f);			// Automatically destroy the bulletType in 1 second.
	}
	
	private void OnTriggerEnter2D (Collider2D col) {
		string name = col.gameObject.name;
		GameObject gO = col.gameObject;
		if (name.Equals("Background") || name.Contains("Access") || name.Contains("Door") || name.Contains("floor"))
			Destroy(gameObject);
		else if (!gO.GetComponent<PolygonCollider2D>().isTrigger) {
			if (col.gameObject.tag.Equals("Enemy"))
				Destroy(gameObject);		
			float dmg = gun.dmgAmounts[gun.bulletType];
			if (name.Contains("Pointy Legs"))
				gO.GetComponent<PointyLegs>().TakeDamage(dmg);
			else if (name.Contains("Four Eyes") && gO.GetComponent<FourEyes>().allowedToDestroy)
				gO.GetComponent<FourEyes>().TakeDamage(dmg);
			else if (name.Contains("Explodetaur"))
				gO.GetComponent<Explodetaur>().TakeDamage(dmg);
		}
	}
}
