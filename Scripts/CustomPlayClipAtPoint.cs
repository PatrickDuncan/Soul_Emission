using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class CustomPlayClipAtPoint : MonoBehaviour {

	// http://forum.unity3d.com/threads/solved-find-all-audiomixergroups.320913/#post-2081399
	public AudioMixer audioMixer;

	public AudioSource PlayClipAt (AudioClip clip, Vector3 pos) {
		try {
		    GameObject tempGO = new GameObject("TempAudio");
		    tempGO.transform.position = pos;
		    AudioSource source = tempGO.AddComponent<AudioSource>();
		    source.clip = clip;
		    // Allows the player to adjust the sound effects' volume.
		    source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SoundFx")[0];
		    source.Play();
		    Destroy(tempGO, clip.length);
		    return source;
	    } catch (Exception e) {
			print(e);
			return null;
		} 
	}
}