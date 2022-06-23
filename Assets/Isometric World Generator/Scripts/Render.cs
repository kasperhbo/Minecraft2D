using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Render : MonoBehaviour {

	[HideInInspector] public WorldCreator world;
	// free indexes in pool to reuse
	private List<int> freeIndexes = new List<int>();
	// is it curently redrawing
	[HideInInspector] public bool isCalculating = false;
	// initial number of tile in viewport X
	[HideInInspector] public int cellNbX				= 6;
	// initial number of tile in viewport y
	[HideInInspector] public int cellNbY				= 6;
	// last camera position cell number in viewport X
	[HideInInspector] public int oldCellNbX				= 0;
	// last camera position cell number in viewport Y
	[HideInInspector] public int oldCellNbY				= 0;
	// object pool
	private List<GameObject> pool = new List<GameObject>();
	// half the number of cell in viewport X
	private float halfCellX;
	// half the number of cell in viewport Y
	private float halfCellY;

	// Use this for initialization
	public void Initialize () {
		world = GetComponent<WorldCreator> ();
		world.Initialize ();
	}

	public void FirstRender() {
		world.SetUpSprites ();
		// update the current cellNbPerScreen
		UpdateCellNb ();
		// initialise the pool and drawing
		SwitchInstances (true);
	}

	public void ClearRender () {
		if (pool.Count > 0) {
			for (int i = 0; i < pool.Count; i++) {
				if (pool[i].activeSelf) {
					pool [i].SetActive (false);
				}
			}
		}
//		pool.Clear ();
//		freeIndexes.Clear ();
		GameManager.instance.forbidInputs = true;
		Initialize ();
		GameManager.instance.forbidInputs = false;
	}


	// manage the objects pool instances
	// first: is the first call (will act slightly differently). 
	public void SwitchInstances(bool first = false) {
		// is it curretnly calculating
		isCalculating = true;
		// dvide camera x by isometric decal x
		float xByDecalX = GameManager.instance.gameInputs.gameCamera.transform.position.x / world.decalX;
		// dvide camera y by isometric decal y
		float yByDecalY = GameManager.instance.gameInputs.gameCamera.transform.position.y / world.decalY;
		// dvide old camera x by isometric decal x
		float oldXByDecalX = GameManager.instance.gameInputs.oldPosition.x / world.decalX;
		// dvide camera y by isometric decal y
		float oldYByDecalY = GameManager.instance.gameInputs.oldPosition.y / world.decalY;
		// calculate half number of cell in X
		float halfOldCellX = (oldCellNbX / 2);
		// calculate half number of cell in y
		float halfOldCellY = (oldCellNbY / 2);

		// bounding box init x position
		float initX = xByDecalX * 0.5f + yByDecalY * 0.5f;
		// bounding box init y position
		float initY = yByDecalY *0.5f - xByDecalX * 0.5f;

		// calculate current viewport
		int[] boundingBox = { (int) (initX  - halfCellX),
			(int) (initX + halfCellX),
			(int) (initY  - halfCellY),
			(int) (initY + halfCellY) };

		boundingBox[0] -= (int) Mathf.Floor((float)(boundingBox[0])*0.2f);
		boundingBox[1] += (int) Mathf.Ceil((float)(boundingBox[1])*0.2f);
		boundingBox[2] -= (int) Mathf.Floor((float)(boundingBox[2])*0.2f);
		boundingBox[3] += (int) Mathf.Ceil((float)(boundingBox[3])*0.2f);

		// old bounding box init x position
		float oldInitX = oldXByDecalX * 0.5f + oldYByDecalY * 0.5f;
		// old bounding box init y position
		float oldInitY = oldYByDecalY *0.5f - oldXByDecalX * 0.5f;

		// calculate previous viewport
		int[] oldBoundingBox = { (int) (oldInitX  - halfOldCellX),
			(int) (oldInitX + halfOldCellX),
			(int) (oldInitY  - halfOldCellY),
			(int) (oldInitY + halfOldCellY)};

		// nb of items freed (debug)
		int free = 0;
		// nb of item without change (debug)
		int noChange = 0;

		// find lower x between low new and old bounding box
		int minX = boundingBox [0];
		if (oldBoundingBox [0] < boundingBox[0]) {
			minX = oldBoundingBox [0];
		}

		// find higher x between high new and old bouding box
		int maxX = boundingBox [1];
		if (oldBoundingBox [1] > boundingBox[1]) {
			maxX = oldBoundingBox [1];
		}

		maxX ++;

		// find lowest y between low new and old bouding box
		int minY = boundingBox [2];
		if (oldBoundingBox [2] < boundingBox[2]) {
			minY = oldBoundingBox [2];
		}

		// find highest y between high new and old bounding box
		int maxY = boundingBox [3];
		if (oldBoundingBox [3] > boundingBox[3]) {
			maxY = oldBoundingBox [3];
		}
		maxY++;

		// find highest x between low new and old bounding box
		int maxX2 = boundingBox [0];
		if (maxX2 < oldBoundingBox [0]) {
			maxX2 = oldBoundingBox [0];
		}
		maxX2++;

		// find highest y between low new and old bounding box
		int maxY2 = boundingBox [2];
		if (maxY2 < oldBoundingBox [2]) {
			maxY2 = oldBoundingBox [2];
		}
		maxY2++;

		// find lowest x between high new and old bounding box
		int minX2 = boundingBox [1];
		if (minX2 > oldBoundingBox [1]) {
			minX2 = oldBoundingBox [1];
		}

		// find lowest y between high new and old bounding box
		int minY2 = boundingBox [3];
		if (minY2 > oldBoundingBox[3]) {
			minY2 = oldBoundingBox [3];
		}

		int _add = 0;

		// create 4 bounding box area which are differences between old
		// and new viewport
		int[] loops = {
			// left
			minX, maxX2, minY, maxY,
			// bottom
			minX, maxX,minY, maxY2,
			// right
			minX2, maxX, minY, maxY,
			// top
			minX, maxX, minY2, maxY
		};

		// if we're running it for the fisrt time
		// just use current viewport
		if (first) {
			loops = boundingBox;
			loops [0]--;
			loops [1]++;
			loops [2]--;
			loops [3]++;
		}


		// loop throught the bounding boxes
		for (int i = 0; i < (int) (loops.Length/4); i++) {
			// if we're out of the map, skip
			if (loops[(i*4)+1] < 0 || loops[(i*4)+3] < 0 ||
				loops[(i*4)+1] == loops[i*4] || loops[(i*4)+3] == loops[(i*4)+2]) {

				continue;
			}
			// if we're below 0, start at 0
			if (loops[i*4] < 0) {
				loops [i * 4] = 0;
			}
			if (loops[(i*4)+2] < 0) {
				loops [(i * 4) + 2] = 0;
			}

			// loop through the part of the grid covered by the bounding box
			for (int x = loops[i*4]; x < loops[(i*4)+1]; x++) {
				for (int y = loops[(i*4)+2]; y < loops[(i*4)+3]; y++) {
					// if we're out of the map or the instance is null, skip
					if (x < 0 || x > world.sizeX - 1 || y < 0 || y > world.sizeY - 1 || 
						world.instances [x, y, 0] == null) {
						continue;
					}

					// if it's in the current viewport
					if (x >= (int)boundingBox [0] &&
						x <= (int)boundingBox [1] &&
						y >= (int)boundingBox [2] &&
						y <= (int)boundingBox [3]) {
						// if it as as well in the old view port
						if (x >= (int)oldBoundingBox [0] &&
							x <= (int)oldBoundingBox [1] &&
							y >= (int)oldBoundingBox [2] &&
							y <= (int)oldBoundingBox [3] && 
							!first) {
							// nothing to do, skip
							noChange++;
							continue;
						}

						// if the current object doesn't have an instance in the pool
						if (world.instanceIndexes [x, y, 0] == -1) {
							// loop through z-index
							for (int z = 0; z < 3; z++) {
								// if there is something there
								if (world.instances [x, y, z] != null) {
									// if there is free index in the pool
									if (freeIndexes.Count > 0) {
										// replace the existing element
										pool [freeIndexes [0]].GetComponent<SpriteRenderer>().sprite = world.instanceSprites[x,y,z];
										pool [freeIndexes [0]].transform.position = world.instancesPositions [x, y, z];

										// if the object is not active, active it
										if (!pool [freeIndexes [0]].activeSelf) {
											pool [freeIndexes [0]].SetActive (true);
										}

										// register the index
										world.instanceIndexes [x, y, z] = freeIndexes [0];
										freeIndexes.RemoveAt (0);

									} else {
										// add a new object to the pool 
										pool.Add ((GameObject)Instantiate (world.instances [x, y, z], world.instancesPositions [x, y, z], Quaternion.identity));
										// register the index
										world.instanceIndexes [x, y, z] = pool.Count - 1;
										_add++;
									}
								}
							}
						}

						// if it's not in the current viewport but have an index on the pool and it's not null
					} else if (world.instanceIndexes [x, y, 0] != -1 && world.instances [x, y, 0] != null) {
						// if it's outside of the old bouding box, skip
						if (x < (int)oldBoundingBox [0] ||
							x > (int)oldBoundingBox [1] ||
							y < (int)oldBoundingBox [2] ||
							y > (int)oldBoundingBox [3]) {
							continue;
						}

						// loop through z-level
						for (int z = 0; z < 3; z++) {
							// if there is something there
							if (world.instances [x, y, z] != null) {
								// if it' active, turn it inactive
								if (pool [world.instanceIndexes [x, y, z]].activeSelf == true) {
									pool [world.instanceIndexes [x, y, z]].SetActive (false);
								}
								// add it as free index
								freeIndexes.Add (world.instanceIndexes [x, y, z]);
								// unregister the index
								world.instanceIndexes [x, y, z] = -1;

								free++;
							}
						}
					}
				}
			}
		}
		// notify you're done calculating 
		isCalculating = false;
		// register the cell nb as old cell number
		oldCellNbX = cellNbX;
		oldCellNbY = cellNbY;
		//		print (GameObject.FindObjectsOfType(typeof(SpriteRenderer)).Length+" free: "+free+" no change: "+noChange+" add:"+_add);
	}
	
	// Update is called once per frame
	void Update () {
	}


	// update the number of cell displayed by screen
	public void UpdateCellNb() {
		// get the number of cell based on screen size.
		cellNbX = (int)((float) (Mathf.Ceil ((Screen.width / 132) * GameManager.instance.gameInputs.gameCamera.orthographicSize) + 2)/1.3f);
		cellNbY = (int)((float) (Mathf.Ceil ((Screen.height / 66) * GameManager.instance.gameInputs.gameCamera.orthographicSize) + 2)/1.3f);
		if (oldCellNbX == 0) {
			oldCellNbX = cellNbX;
		}
		if (oldCellNbY == 0) {
			oldCellNbY = cellNbY;
		}
		// calculate half of it.
		halfCellX = (cellNbX / 2);
		halfCellY = (cellNbY / 2);
	}
}
