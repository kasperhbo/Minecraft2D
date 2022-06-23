using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsHandler : MonoBehaviour {
	// camera
	[HideInInspector] public Camera gameCamera;
	// camera othographic size
	private float size					= 5f;
	// last camera position
	[HideInInspector] public Vector3 oldPosition;

	public float maxUnzoom 				= 20f;
	public float minUnzoom 				= 4f;
	public int mouseWheelZoomMultiplier = 10;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize() {
		gameCamera = Camera.main;
		gameCamera.farClipPlane = 100000f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.forbidInputs) {
			int horizontal = 0;  	//Used to store the horizontal move direction.
			int vertical = 0;
			// get horizontal and vertical movement
			horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
			vertical = (int)(Input.GetAxisRaw ("Vertical"));
			// get camera orthographic size
			size = gameCamera.orthographicSize;

			if (Input.GetKeyDown (KeyCode.Space)) {
				GameManager.instance.gameRenderer.ClearRender ();

			}

			// if we moved
			if (horizontal != 0 || vertical != 0) {
				// place the camera at new position
				gameCamera.transform.position = new Vector3 (gameCamera.transform.position.x + (horizontal)*(Time.deltaTime*(10+size)),
					gameCamera.transform.position.y + vertical*(Time.deltaTime*(10+size)), -1);
			}

			// change the size if mousewheel is used
			gameCamera.orthographicSize -= Input.GetAxis ("Mouse ScrollWheel") * mouseWheelZoomMultiplier;

			// don't allow zoom below 4
			if (gameCamera.orthographicSize < minUnzoom) {
				gameCamera.orthographicSize = minUnzoom;
			}
			// don't allow unzoom above 20
			if (gameCamera.orthographicSize > maxUnzoom) {
				gameCamera.orthographicSize = maxUnzoom;
			}

			// if camera size changed, update the cell number per viewport
			if (gameCamera.orthographicSize != size) {
				GameManager.instance.gameRenderer.UpdateCellNb ();
			}

			// if we moved, or we zoom / unzoomed, redraw the correct objects.
			if (gameCamera.transform.position.x >= oldPosition.x + 4 || 
				gameCamera.transform.position.x <= oldPosition.x - 4 ||
				gameCamera.transform.position.y >= oldPosition.y + 4 ||
				gameCamera.transform.position.y <= oldPosition.y - 4 ||
			   	gameCamera.orthographicSize != size) {

				if (!GameManager.instance.gameRenderer.isCalculating) {
					GameManager.instance.gameRenderer.SwitchInstances ();
					// register camera position as old position
					oldPosition = gameCamera.transform.position;
				}
			}
		}
	}
}
