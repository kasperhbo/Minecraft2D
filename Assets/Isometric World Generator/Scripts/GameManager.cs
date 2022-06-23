using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;
	[HideInInspector] public Render gameRenderer;
	[HideInInspector] public InputsHandler gameInputs;
	[HideInInspector] public LoadingWorldUI gameLoadingWorldUI;
	[HideInInspector] public bool forbidInputs = false;


	//Awake is always called before any Start functions
	void Awake()
	{
		gameRenderer = GetComponent<Render> ();
		gameInputs = GetComponent<InputsHandler> ();
		gameLoadingWorldUI = GetComponent<LoadingWorldUI> (); 

		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);	
		
		gameInputs.Initialize ();
		gameRenderer.Initialize ();
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (gameRenderer.world.generateProgress > -1) {
			if (gameLoadingWorldUI != null && gameLoadingWorldUI.enabled == true) {
				gameLoadingWorldUI.UpdateProgress (gameRenderer.world.generateProgress, 
					gameRenderer.world.generateProgressText);
			}
			if (gameRenderer.world.generateProgress == 100) {
				if (gameLoadingWorldUI != null && gameLoadingWorldUI.enabled == true) {
					gameLoadingWorldUI.Discard ();
				}
				gameRenderer.FirstRender ();
				gameRenderer.world.generateProgress = -1;
			}
		}
	}
}
