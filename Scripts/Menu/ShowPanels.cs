using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public bool fromPause;					// Whether you got to a panel from the pause menu.

	public GameObject controlsPanel;		// Reference to the Game Object ControlsPanel. 
	public GameObject optionsPanel;			// Reference to the Game Object OptionsPanel.
	public GameObject creditsPanel;			// Reference to the Game Object CreditsPanel.
	public GameObject Tint;					// Reference to the Game Object Tint.
	public GameObject menuPanel;			// Reference to the Game Object MenuPanel.
	public GameObject pausePanel;			// Reference to the Game Object PausePanel.
	public GameObject PC1;					// Reference to the Game Object PC1.
	public GameObject PC2;					// Reference to the Game Object PC2.
	public GameObject Mobile1;				// Reference to the Game Object Mobile1.
	public GameObject Mobile2;				// Reference to the Game Object Mobile2.
	public GameObject[] panels; 			// Array of the panels initialized above.

	private void Awake () {
		panels = new GameObject[] {PC1, PC2, Mobile1, Mobile2, controlsPanel, optionsPanel, creditsPanel};
	}

	private void Update () {
		// Allows you to go back out of any meny with an input
		if (Input.GetButtonDown("Cancel")) {
			foreach (GameObject panel in panels) {
				if (panel.activeSelf) {
					Hide(panel);
					break;
				}
			}	
		} 
	}

	public void Show (GameObject panel) {
		panel.SetActive(true);
		Tint.SetActive(true);
		if (pausePanel.activeSelf && panel != pausePanel) {
			fromPause = true;
			Hide(pausePanel);
		} else 
			fromPause = false;
	}

	public void Hide (GameObject panel) {
		panel.SetActive(false);
		if (fromPause && panel != pausePanel)
			Show(pausePanel);
		else if (panel != PC1 && panel != PC2 && panel != Mobile1 && panel != Mobile2) {
			if (!fromPause)
				Tint.SetActive(false);
		}
	}

	public void Switch (GameObject panel) {
		if (panel == PC1) 
			PC2.SetActive(true);
		else if (panel == PC2) 
			PC1.SetActive(true);
		else if (panel == Mobile1) 
			Mobile2.SetActive(true);
		else if (panel == Mobile2) 
			Mobile1.SetActive(true);
		panel.SetActive(false);
	}

	public void ShowMenu () {
		menuPanel.SetActive (true);
	}

	public void HideMenu () {
		menuPanel.SetActive (false);
	}	
}
