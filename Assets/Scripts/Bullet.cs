using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private void Start () {
		Destroy(gameObject, 1f);		//Automatically destroy the bullet in 1 second.
	}
	
	private void OnTriggerEnter2D (Collider2D col) {
		string tag = col.gameObject.tag;
		if (tag == "Background") 
			Destroy(gameObject);
		else if (tag.Contains("Stairs") && !GameObject.FindGameObjectWithTag(tag).GetComponent<PolygonCollider2D>().isTrigger)
			Destroy(gameObject);
		else if (tag.Contains("PointyLegs") && !GameObject.FindGameObjectWithTag(tag).GetComponent<PolygonCollider2D>().isTrigger) {
			GameObject.FindGameObjectWithTag(tag).GetComponent<PointyLegs>().TakeDamage(10f);
			Destroy(gameObject);
		}
	}
}
