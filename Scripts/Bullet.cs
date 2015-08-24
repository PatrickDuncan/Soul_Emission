using UnityEngine;

public class Bullet : MonoBehaviour {

	private void Start () {
		Destroy(gameObject, 0.7f);		// Automatically destroy the bullet in 1 second.
	}
	
	private void OnTriggerEnter2D (Collider2D col) {
		string name = col.gameObject.name;
		GameObject gO = col.gameObject;
		if (name.Equals("Background") || name.Contains("Access") || name.Contains("Door") || name.Contains("floor"))
			Destroy(gameObject);
		else if (!gO.GetComponent<PolygonCollider2D>().isTrigger) {
			if (col.gameObject.tag.Equals("Enemy"))
				Destroy(gameObject);		
			if (name.Contains("Pointy Legs"))
				gO.GetComponent<PointyLegs>().TakeDamage(7f);
			else if (name.Contains("Four Eyes") && gO.GetComponent<FourEyes>().allowedToDestroy)
				gO.GetComponent<FourEyes>().TakeDamage(7f);
			else if (name.Contains("Explodetaur"))
				gO.GetComponent<Explodetaur>().TakeDamage(7f);
		}
	}
}
