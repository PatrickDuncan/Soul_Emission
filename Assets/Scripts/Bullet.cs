using UnityEngine;

public class Bullet : MonoBehaviour {

	private Gun gun;						// Reference to the Gun class in Player.

	private void Start () {
		gun = GameObject.FindWithTag("Player").GetComponentInChildren<Gun>();
		Destroy(gameObject, 0.8f);			// Automatically destroy the bulletType in 1 second.
	}
	
	private void OnTriggerEnter2D (Collider2D col) {
		string name = col.gameObject.name;
		GameObject gO = col.gameObject;
		if (name.Equals("Background") || name.Contains("Access") || name.Contains("Enter") || name.Contains("Exit") || name.Contains("floor"))
			Destroy(gameObject);
		else if (!gO.GetComponent<PolygonCollider2D>().isTrigger) {
			float dmg = gun.dmgAmounts[gun.bulletType];
			if (name.Contains("Pointy Legs") && gO.GetComponent<PointyLegs>().health > 0f) {
				gO.GetComponent<PointyLegs>().TakeDamage(dmg);
				Destroy(gameObject);
			}
			else if (name.Contains("Four Eyes") && gO.GetComponent<FourEyes>().health > 0f && gO.GetComponent<FourEyes>().allowedToDestroy) {
				gO.GetComponent<FourEyes>().TakeDamage(dmg);
				Destroy(gameObject);
			}
			else if (name.Contains("Explodetaur") && gO.GetComponent<Explodetaur>().health > 0f) {
				gO.GetComponent<Explodetaur>().TakeDamage(dmg);
				Destroy(gameObject);
			}
		}
	}
}
		