using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private bool isPaused;						// Boolean to check if the game is paused or not.
	private AudioSource[] allAudioSources;		// All audio sources in the scene.

	private ShowPanels showPanels;				// Reference to the ShowPanels script used to hide and show UI panels.
	private StartOptions startScript;			// Reference to the StartButton script.
	public GameObject pausePanel;				// Store a reference to the Game Object PausePanel.
	
	private void Awake () {
		showPanels = GetComponent<ShowPanels>();
		startScript = GetComponent<StartOptions>();
	}

	private void Update () {
		if (Input.GetButtonDown("Cancel") && !isPaused && !startScript.inMainMenu && !showPanels.fromPause) {
			DoPause();
		} 
		else if (Input.GetButtonDown("Cancel") && isPaused && !startScript.inMainMenu && !showPanels.fromPause && !showPanels.controls) {
			UnPause();
		}
	}

	public void DoPause () {
		isPaused = true;
		//Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource sound in allAudioSources) {
			sound.Pause();
		}
		showPanels.Show(pausePanel);

	}

	public void UnPause () {
		isPaused = false;
		// Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
		Time.timeScale = 1;
		foreach (AudioSource sound in allAudioSources) {
			sound.UnPause();
		}
		showPanels.Hide(pausePanel);
	}
}
