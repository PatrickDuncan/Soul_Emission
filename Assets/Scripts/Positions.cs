using UnityEngine;
using System.Collections;
using System;

public class Positions : MonoBehaviour {

	private string playerPos;
	private string pointyPos;
	public Vector3 player;
	public Vector3[] pointy;
	public bool isRight;
	public int pointyStart;			// The value to start cycling through the pointy leg tags.

	private void Awake () {
		TextAsset asset = Resources.Load("playerPos") as TextAsset;
		playerPos = asset.text;
		asset = Resources.Load("pointyPos") as TextAsset;
		pointyPos = asset.text;
		player = computePlayer();
		pointy = new Vector3[0];
		pointyStart = 1;
		pointy = compute(pointy, pointyPos);
	}

	private Vector3 computePlayer () {
		string[] split = playerPos.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
		int i=0;
		foreach (string s in split) {
			try {
				// Make sure you get the position for the correct level
				if (s.Length == 1 && Convert.ToInt32(s) == Application.loadedLevel) {
					// The next 2 strings are the x and y, convert to float then return Vector3
					string[] info = split[i+1].Split(' ');
					isRight = Convert.ToBoolean(info[2]);
					return new Vector3(Convert.ToSingle(info[0]), Convert.ToSingle(info[1]), 0f);
				}
				i++;
			}
			catch (Exception e) {
				print(e);
			}
		}
		return new Vector3 (0f, 0f, 0f);
	}

	private Vector3[] compute (Vector3[] vector, string file) {
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
				else if (s.Length != 1)
					pointyStart++;
				i++;
			}
			
		}
		catch (Exception e) {
			print(e);
		}
		return vector;
	}
}
