using UnityEngine;

public class HelpfulTips : MonoBehaviour {
	
	public GameObject Help1;				// Help tip game object
	public GameObject Help2;				// Help tip game object
	public GameObject Help3;				// Help tip game object
	public GameObject Help4;				// Help tip game object
	public GameObject Help5;				// Help tip game object

	public void Show (int index) {
		GameObject[] tips = {Help1, Help2, Help3, Help4, Help5};
		if (index != -1)
			tips[index].SetActive(true);
		for (int i=0; i<5; i++) {
			if (i != index) {
				tips[i].SetActive(false);	
			}
		}
	}
}
