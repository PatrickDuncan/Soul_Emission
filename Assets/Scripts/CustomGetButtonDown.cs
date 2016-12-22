using UnityEngine;
using System.Collections;

public class CustomGetButtonDown : MonoBehaviour {

	// This class allows you to hold down the shoot button to fire automatically.
	// Once you press down the correct key it will set a bool true and that bool
	// will only be set false when you release the key.

	private static bool shot;					// If you've just shot the gun.

	public static bool ButtonDown () {
		if (Input.GetButtonDown("Shoot"))
			shot = true;
		return shot;
	}

	public static void ButtonUp () {
		if (Input.GetButtonUp("Shoot"))
			shot = false;
	}
}