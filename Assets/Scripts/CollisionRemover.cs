using UnityEngine;
using System.Collections;

public class CollisionRemover : MonoBehaviour {		

	PlayerControl playerCtrl;			// Reference to the PlayerControl script.
	PointyLegs pointy;					// Reference to the PointyLegs script.

	private void Awake () {
		playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
		pointy = GameObject.FindGameObjectWithTag("PointyLegs1").GetComponent<PointyLegs>();
	}

	private void OnMouseDown () {
		if (!playerCtrl.stairsTag.Equals(transform.tag) && !pointy.stairsTag.Equals(transform.tag)) {
			GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
			GetComponent<PolygonCollider2D>().usedByEffector = !GetComponent<PolygonCollider2D>().usedByEffector;
		}
	}

	private void OnTouchStart () {
		if (playerCtrl.stairsTag.Equals(pointy.stairsTag) && !playerCtrl.stairsTag.Equals("none")) {
			GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
			GetComponent<PolygonCollider2D>().usedByEffector = !GetComponent<PolygonCollider2D>().usedByEffector;
		}
	}		
}
