using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class StartOptions : MonoBehaviour {

	public int sceneToStart = 1;			// Index number in build settings of scene to load if changeScenes is true
	public bool changeScenes;				// If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart;			// Choose whether to continue playing menu music or start a new music clip
	[HideInInspector] 
	public bool inMainMenu = true;			// If true, pause button disabled in main menu  //(Cancel in input manager, default escape key)]
	
	private ShowPanels showPanels;			// Reference to ShowPanels script on UI GameObject, to show and hide panels
	
	void Awake () {
		showPanels = GetComponent<ShowPanels> ();
	}

	public void StartButtonClicked () {
		GetComponent<Animator>().SetTrigger("Fade");
		Invoke("MoveOn", 1f);		
	}

	private void MoveOn () {
		GetComponent<AudioSource>().Stop();
		showPanels.HideMenu();
		inMainMenu = false;
		Application.LoadLevel(sceneToStart);
	}
}
