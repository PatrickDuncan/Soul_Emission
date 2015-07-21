using UnityEngine;
using System.Collections;

public class CollisionRemover : MonoBehaviour {		

	private void OnMouseDown () {
		GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
		GetComponent<PolygonCollider2D>().usedByEffector = !GetComponent<PolygonCollider2D>().usedByEffector;
	}

	private void OnTouchStart () {
		GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
		GetComponent<PolygonCollider2D>().usedByEffector = !GetComponent<PolygonCollider2D>().usedByEffector;
	}		
}
