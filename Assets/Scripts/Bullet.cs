using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private PointyLegs pointy;

	private void Start () {
		for (int i=1; i<5; i++) {
			try {
				pointy = GameObject.FindGameObjectWithTag("PointyLegs"+i).GetComponent<PointyLegs>();
			}
			catch (UnityEngine.UnityException) {
				break;
			}
		}
		Destroy(gameObject, 1);
	}
	
	private void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Background") {
			Destroy(gameObject);
		}
		for (int i=1; i<10; i++) {
			try {
				if (col.gameObject.tag == "Stairs"+i && !GameObject.FindGameObjectWithTag("Stairs"+i).GetComponent<PolygonCollider2D>().isTrigger) {
					Destroy(gameObject);
					break;		//Can only collide with one stairs at a time
				}
			}
			catch (UnityEngine.UnityException) {
				break;
			}
			try {
				if (col.gameObject.tag == "PointyLegs"+i && !GameObject.FindGameObjectWithTag("PointyLegs"+i).GetComponent<PolygonCollider2D>().isTrigger) {
					pointy.TakeDamage();
					Destroy(gameObject);
				}
			}
			catch(UnityEngine.UnityException) {
				break;
			}
		}
	}
}
