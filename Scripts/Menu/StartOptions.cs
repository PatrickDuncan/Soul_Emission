using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class StartOptions : MonoBehaviour {

	public int sceneToStart = 1;			// Index number in build settings of scene to load.
	[HideInInspector] 
	public bool inMainMenu = true;			// If true, pause button disabled in main menu  //(Cancel in input manager, default escape key)]
	
	private ShowPanels showPanels;			// Reference to ShowPanels script on UI GameObject, to show and hide panels
	
	void Awake () {
		showPanels = GetComponent<ShowPanels>();
	}

	public void StartButtonClicked () {
		if (inMainMenu) {
			inMainMenu = false;
			GetComponent<Animator>().SetTrigger("Fade");
			Invoke("MoveOn", 1f);		
		}
	}

	private void MoveOn () {
		showPanels.ToggleLoading(true);
		showPanels.HideMenu();
		Application.LoadLevel(sceneToStart);
	}
}
