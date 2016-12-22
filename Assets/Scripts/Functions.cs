using UnityEngine;
using System;

public static class Functions {

	// Gets the distance between 2 objects and checks if less than the max.
	public static bool DeltaMax (float pos1, float pos2, float max) {
		return Mathf.Abs(pos2 - pos1) < max;
	}

	public static bool DeltaMin (float pos1, float pos2, float min) {
		return Mathf.Abs(pos2 - pos1) > min;
	}
	
	// fullPathHash gets the current animation clip.
	public static int GetPath (Animator anim) {
		return anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
	}
}