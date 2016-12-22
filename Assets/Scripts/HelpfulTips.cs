using UnityEngine;

public class HelpfulTips : MonoBehaviour {
	
	public GameObject Help1;				// Helpful tip game object.
	public GameObject Help2;				// Helpful tip game object.
	public GameObject Help3;				// Helpful tip game object.
	public GameObject Help4;				// Helpful tip game object.
	public GameObject Help5;				// Helpful tip game object.
	public GameObject Help6;				// Helpful tip game object.
	public GameObject End;					// End of demo overlay.

	public void Show (int index) {
		GameObject[] tips = {Help1, Help2, Help3, Help4, Help5, Help6, End};
		if (index != -1)
			tips[index].SetActive(true);
		// Loop through all the help objects and set the ones not being used to false.
		for (int i=0; i<tips.Length; i++) {
			if (i != index)
				tips[i].SetActive(false);	
		}
	}
}
