using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private void Start () {
		Destroy(gameObject, 0.9f);		// Automatically destroy the bullet in 1 second.
	}
	
	private void OnTriggerEnter2D (Collider2D col) {
		string tag = col.gameObject.tag;
		if (tag == "Background") 
			Destroy(gameObject);
		else if (tag.Contains("PointyLegs") && !GameObject.FindWithTag(tag).GetComponent<PolygonCollider2D>().isTrigger) {
			GameObject.FindWithTag(tag).GetComponent<PointyLegs>().TakeDamage(10f);
			Destroy(gameObject);
		}
		else if (tag.Contains("FourEyes") && !GameObject.FindWithTag(tag).GetComponent<PolygonCollider2D>().isTrigger && GameObject.FindWithTag(tag).GetComponent<FourEyes>().allowedToDestroy) {
			GameObject.FindWithTag(tag).GetComponent<FourEyes>().TakeDamage(10f);
			Destroy(gameObject);
		}
	}
}
