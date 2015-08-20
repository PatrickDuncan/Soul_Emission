using UnityEngine;

public interface IEnemy {
	
	void TakeDamage(float damage);
	void OnTriggerEnter2D(Collider2D col);
}