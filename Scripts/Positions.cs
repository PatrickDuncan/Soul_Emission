using UnityEngine;
using System;

// In the Background object because it will always be loaded in every level/scene.
public class Positions : MonoBehaviour {

	private string playerPos;		// All the positions for the player.
	private string pointyPos;		// All the positions for the pointy legs.
	private string fourEyesPos;		// All the positions for the four eyes.
	public Vector3 player;			// The position for this level.
	public Vector3[] pointy;		// The positions for this level for the pointy legs.
	public Vector3[] fourEyes;		// The positions for this level for the four eyes.
	public bool isRight;			// Whether the player is facing right.

	private void Awake () {
		TextAsset asset = Resources.Load("playerPos") as TextAsset;
		playerPos = asset.text;
		asset = Resources.Load("pointyPos") as TextAsset;
		pointyPos = asset.text;
		asset = Resources.Load("fourEyesPos") as TextAsset;
		fourEyesPos = asset.text;
		GetPositions();
	}

	private void OnLevelWasLoaded () {
		GetPositions();
	}

	private void GetPositions () {
		player = ComputePlayer();
		pointy = new Vector3[0];
		fourEyes = new Vector3[0];
		pointy = Compute(pointy, pointyPos, "pointy");
		fourEyes = Compute(fourEyes, fourEyesPos, "fourEyes");
	}

	private Vector3 ComputePlayer () {
		try {
			string[] split = playerPos.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			int i=0;
			foreach (string s in split) {			
				// Make sure you get the position for the correct level
				if (s.Length == 1 && Convert.ToInt32(s) == Application.loadedLevel) {
					// The next 2 strings are the x and y, convert to float then return Vector3
					string[] info = split[i+1].Split(' ');
					isRight = Convert.ToBoolean(info[2]);
					return new Vector3(Convert.ToSingle(info[0]), Convert.ToSingle(info[1]), 0f);
				}
				i++;
			}
		}
		catch (Exception e) {
			print(e);
		}
		return new Vector3 (0f, 0f, 0f);
	}

	private Vector3[] Compute (Vector3[] vector, string file, string enemy) {
		try {
			string[] split = file.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			int i=0;
			foreach (string s in split) {
				// Make sure you get the position for the correct level
				if (s.Length == 1 && Convert.ToInt32(s) == Application.loadedLevel) {
					i++;
					string next = split[i];
					while (next.Length != 1) {
						Array.Resize(ref vector, vector.Length + 1);
						// The next 2 strings are the x and y, convert to float then return Vector3
						string[] info = split[i].Split(' ');
						vector[vector.Length-1] = new Vector3(Convert.ToSingle(info[0]), Convert.ToSingle(info[1]), 0.6f);
						// Advance to the next string in the file.
						i++;
						next = split[i];
					}
					break;				
				}
				i++;
			}
		}
		catch (Exception e) {
			print(e);
		}
		return vector;
	}
}
