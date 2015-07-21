using UnityEngine;
using System.Collections;

public class CollisionRemover : MonoBehaviour {		

	PlayerControl playerCtrl;
	PointyLegs pointy;

	private void Awake () {
		playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
		pointy = GameObject.FindGameObjectWithTag("PointyLegs1").GetComponent<PointyLegs>();
	}

	private void OnMouseDown () {
		if (!playerCtrl.inStairs && !pointy.inStairs) {
			GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
			GetComponent<PolygonCollider2D>().usedByEffector = !GetComponent<PolygonCollider2D>().usedByEffector;
		}
	}

	private void OnTouchStart () {
		if (!playerCtrl.inStairs && !pointy.inStairs) {
			GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
			GetComponent<PolygonCollider2D>().usedByEffector = !GetComponent<PolygonCollider2D>().usedByEffector;
		}
	}		
}
