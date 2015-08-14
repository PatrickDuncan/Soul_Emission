using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class StartOptions : MonoBehaviour {

	public int sceneToStart = 1;				// Index number in build settings of scene to load if changeScenes is true
	public bool changeScenes;					// If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart;				// Choose whether to continue playing menu music or start a new music clip

	[HideInInspector] 
	public bool inMainMenu = true;				// If true, pause button disabled in main menu  //(Cancel in input manager, default escape key)]

	private ShowPanels showPanels;				// Reference to ShowPanels script on UI GameObject, to show and hide panels
	
	void Awake () {
		//Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<ShowPanels> ();
		//Get a reference to PlayMusic attached to UI object
	}

	public void StartButtonClicked () {
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;
		//Hide the main menu UI element
		showPanels.HideMenu();
		//Load the selected scene, by scene index number in build settings
		Application.LoadLevel(sceneToStart);
	}
}
