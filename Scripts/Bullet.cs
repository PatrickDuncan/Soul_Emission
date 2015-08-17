using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private void Start () {
		Destroy(gameObject, 0.9f);		// Automatically destroy the bullet in 1 second.
	}
	
	private void OnTriggerEnter2D (Collider2D col) {
		string tag = col.gameObject.name;
		if (tag.Equals("Background") || tag.Contains("Access") || tag.Contains("Door") || tag.Contains("floor"))
			Destroy(gameObject);
		else if (tag.Contains("Pointy Legs") && !GameObject.Find(tag).GetComponent<PolygonCollider2D>().isTrigger) {
			GameObject.Find(tag).GetComponent<PointyLegs>().TakeDamage(7f);
			Destroy(gameObject);
		}
		else if (tag.Contains("Four Eyes") && !GameObject.Find(tag).GetComponent<PolygonCollider2D>().isTrigger && GameObject.Find(tag).GetComponent<FourEyes>().allowedToDestroy) {
			GameObject.Find(tag).GetComponent<FourEyes>().TakeDamage(7f);
			Destroy(gameObject);
		}
	}
}
