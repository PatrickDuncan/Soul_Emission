using UnityEngine;

public class ArbitraryTurtleLink : MonoBehaviour {

	public void OnMouseDown () {
		Application.OpenURL("http://arbitraryturtle.github.io");
	}

	public void OnTouchStart () {
		Application.OpenURL("http://arbitraryturtle.github.io");	
	}
}
