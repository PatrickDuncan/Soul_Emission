using UnityEngine;
using System.Collections;

public interface IEnemy {
	
	void TakeDamage(float damage);
	IEnumerator Death();
	void OnTriggerEnter2D(Collider2D col);
}