using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using Random = System.Random;

public class WorldCreator : MonoBehaviour {

	// flat grass tile
	public GameObject flatGrass;
	// palm tree
	public GameObject palmTree;
	// flat sea tile
	public GameObject flatSea;
	// beach SW tile
	public GameObject beachSW;
	// beach NW tile
	public GameObject beachNW;
	// beach SE tile
	public GameObject beachSE;
	// beach NE tile
	public GameObject beachNE;
	// beach S tile
	public GameObject beachS;
	// beach N tile
	public GameObject beachN;
	// beach E tile
	public GameObject beachE;
	// beach W tile
	public GameObject beachW;
	// beach next to desert S tile
	public GameObject beachDesertS;
	// beach next to desert N tile
	public GameObject beachDesertN;
	// beach next to desert E tile
	public GameObject beachDesertE;
	// beach next to desert W tile
	public GameObject beachDesertW;
	// grass next to desert S tile
	public GameObject grassDesertS;
	// grass next to desert N tile
	public GameObject grassDesertN;
	// grass next to desert E tile
	public GameObject grassDesertE;
	// grass next to desert W tile
	public GameObject grassDesertW;
	// grass next to desert SW tile
	public GameObject grassDesertSW;
	// grass next to desert NW tile
	public GameObject grassDesertNW;
	// grass next to desert SE tile
	public GameObject grassDesertSE;
	// grass next to desert NE tile
	public GameObject grassDesertNE;
	// grass next to desert inner corner N tile
	public GameObject grassInnerDesertN;
	// grass next to desert inner corner S tile
	public GameObject grassInnerDesertS;
	// grass next to desert inner corner W tile
	public GameObject grassInnerDesertW;
	// grass next to desert inner corner E tile
	public GameObject grassInnerDesertE;
	// beach next to anything inner corner S tile
	public GameObject beachInnerW;
	// beach next to anything inner corner E tile
	public GameObject beachInnerE;
	// beach next to anything inner corner N tile
	public GameObject beachInnerN;
	// small ramp grass leading to NE
	public GameObject smallGrassUpNE;
	// small ramp grass leading to NW
	public GameObject smallGrassUpNW;
	// small ramp grass leading to SE
	public GameObject smallGrassUpSE;
	// small ramp grass leading to SW
	public GameObject smallGrassUpSW;
	// big ramp grass leading to NE
	public GameObject bigGrassUpNE;
	// big ramp grass leading to NW
	public GameObject bigGrassUpNW;
	// big ramp grass leading to NE
	public GameObject bigGrassUpSE;
	// big ramp grass leading to SW
	public GameObject bigGrassUpSW;
	// big ramp grass leading to S
	public GameObject bigGrassUpS;
	// big ramp grass leading to N
	public GameObject bigGrassUpN;
	// big ramp grass leading to E
	public GameObject bigGrassUpE;
	// big ramp grass leading to W
	public GameObject bigGrassUpW;
	// stack filler object
	public GameObject filler;
	// river filler object SW
	public GameObject fillerRiverSW;
	// river filler object SE
	public GameObject fillerRiverSE;
	// river filler object Horizontal
	public GameObject riverHor;
	// river filler object Vertical
	public GameObject riverVer;
	// river turn tile E
	public GameObject riverTurnE;
	// river turn tile W
	public GameObject riverTurnW;
	// river turn tile N
	public GameObject riverTurnN;
	// river turn tile S
	public GameObject riverTurnS;
	// river corner tile E
	public GameObject riverCornerE;
	// river corner tile W
	public GameObject riverCornerW;
	// river corner tile N
	public GameObject riverCornerN;
	// river corner tile S
	public GameObject riverCornerS;
	// river inner corner tile S
	public GameObject riverInnerCornerS;
	// river inner corner tile N
	public GameObject riverInnerCornerN;
	// river inner corner tile E
	public GameObject riverInnerCornerE;
	// river inner corner tile W
	public GameObject riverInnerCornerW;
	// river tile SE
	public GameObject riverSE;
	// river tile NE
	public GameObject riverNE;
	// river tile SW
	public GameObject riverSW;
	// river tile NW
	public GameObject riverNW;
	// small river ramp tile up NE
	public GameObject smallRiverUpNE;
	// small river ramp tile up NW
	public GameObject smallRiverUpNW;
	// small river ramp tile up SE
	public GameObject smallRiverUpSE;
	// small river ramp tile up SW
	public GameObject smallRiverUpSW;
	// big river ramp tile up NE
	public GameObject bigRiverUpNE;
	// big river ramp tile up NW
	public GameObject bigRiverUpNW;
	// big river ramp tile up SE
	public GameObject bigRiverUpSE;
	// big river ramp tile up SW
	public GameObject bigRiverUpSW;
	// trees object (2 per family) ex:indexes: [0,1] [2,3]
	public GameObject[] trees;
	// sand desert tile
	public GameObject sand;
	// stack object length = highest peak. one index per height
	public GameObject[] stackDirt;
	// stack object waterfall Horizontal length = highest peak. one index per height
	public GameObject[] stackWaterfallHor;
	// stack object water length = highest peak. one index per height
	public GameObject[] stackWater;
	// stack object waterfall vertical length = highest peak. one index per height
	public GameObject[] stackWaterfallVer;

	// display sea tiles
	public bool showSeaTile = false;

	// sea lvl between 0-1 (more sea less land)
	public float seaLvl 				= 0.7f;
	// map size x
	public int sizeX 					= 50;
	// map size y
	public int sizeY 					= 50;
	// isometric decaling factor on x axis
	[HideInInspector] public float decalX = 0.66f;
	// isometric decaling factor on y axis
	[HideInInspector] public float decalY = .33f;
	// default z-index
	[HideInInspector] public int defaultZ = 0;
	// highest mountains
	public int highest					= 10;
	// the bigger the biggest landmass, the small the more small islands
	public int inverseGranularity 		= 20;
	// initial mountains chains number
	public int mountainsNumber			= 5;
	// desert numbers
	public int desertNumber				= 3;
	// percentage of desert on land tile
	public int desertPercent 			= 10;
	// percentage of forest on land tile
	public int forestPercent			= 20;
	// rivers numbers
	public int riversNb 				= 5;
	[HideInInspector] public int generateProgress = -1;
	[HideInInspector] public System.String generateProgressText = "Shaping continents";
	// main grid
	[HideInInspector] public int[,] grid;
	// objects grid (trees,rocks,characters,buildings)
	[HideInInspector] public int[,] objects;
	// list of grass tiles
	private List<Vector2> grass;
	// list of desert tiles
	private List<Vector2> desertsSeeds;
	// list of forests tiles
	private List<Vector2> forestsSeeds;
	// list of rivers tiles
	private List<Vector2> rivers;
	// initialise list of mountains top
	private List<Vector2> tops;
	private int DESERT;
	private const int FLAT_GRASS = 1;
	private const int SEA = 0;
	private int PALM_TREE;
	private Thread generateWorld; 

	// sprites instance on grid position
	[HideInInspector] public GameObject[,,] instances;
	// sprite position on grid position
	[HideInInspector] public Vector3[,,] instancesPositions;
	// instance index in pool
	[HideInInspector] public  int[,,] instanceIndexes;
	// sprites instance on grid position
	[HideInInspector] public  Sprite[,,] instanceSprites;

	// translation for a nine squared area around subject
	private Vector2[] translations = {
		// NW
		new Vector2 (-1, 1),
		// N
		new Vector2 (0, 1),
		// NE
		new Vector2 (1, 1),
		// W
		new Vector2 (-1, 0),
		// center
		new Vector2 (0, 0),
		// E
		new Vector2 (1, 0),
		// SW
		new Vector2 (-1, -1),
		// S
		new Vector2 (0, -1),
		// SE
		new Vector2 (1, -1)
	};

	// will use <= when looking for comparison
	private const int INFERIOR_EQUAL = -2;
	// will use < when looking for comparison
	private const int INFERIOR		 = -1;
	// will use == when looking for comparison
	private const int EQUAL 		 = 0;
	// will use > when looking for comparison
	private const int SUPERIOR		 = 1;
	// will use >= when looking for comparison
	private const int SUPERIOR_EQUAL = 2;

	// mask to look into the four directon around position (N,S,E,W)
	private static readonly int[] FOUR_DIRECTION_MASK 		= new int[] {0,1,0,1,0,1,0,1,0};
	// mask to look into the height directon around position (NW,N,NE,E,W,SW,S,SE)
	private static readonly int[] HEIGHT_DIRECTION_MASK 	= new int[] {1,1,1,1,0,1,1,1,1};
	// mask to look into all nine positions directon around position (NW,N,NE,E,C,W,SW,S,SE)
	private static readonly int[] NINE_POSITIONS_MASK 		= new int[] {1,1,1,1,1,1,1,1,1};
	// mask to look into the north west corner around position (NW,N,W)
	private static readonly int[] NW_CORNER 				= new int[] {1,1,0,1,0,0,0,0,0};
	// mask to look into the south west corner around position (SW,S,W)
	private static readonly int[] SW_CORNER 				= new int[] {0,0,0,1,0,0,1,1,0};
	// mask to look into the north east corner around position (N,NE,E)
	private static readonly int[] NE_CORNER 				= new int[] {0,1,1,0,0,1,0,0,0};
	// mask to look into the south east corner around position (S,SE,E)
	private static readonly int[] SE_CORNER 				= new int[] {0,0,0,0,0,1,0,1,1};

	// generate the grid (continents, desert, forest, mountains, rivers)
	public void GenerateWorld() {
		DESERT = highest + 1;
		PALM_TREE = trees.Length+1;

		grass 		 = new List<Vector2> ();
		rivers 		 = new List<Vector2> ();
		desertsSeeds = new List<Vector2> ();
		forestsSeeds = new List<Vector2> ();
		tops		 = new List<Vector2> ();

		Random randomSeed = new Random ();
		// calculate land mass from sea level
		float land = 1 - seaLvl;
		// calculate land tiles number
		int landTilesNb = (int)(sizeX * sizeY * land);

		// place land tile
		for (int i = 0; i < landTilesNb; i++) {
			// check if it will be placed randomly or next to another one
			int placement = randomSeed.Next(0, inverseGranularity);
			// calculate random x
			int x = randomSeed.Next((int)sizeX / 10, sizeX - ((int)sizeX / 10));
			// calculate random y
			int y = randomSeed.Next((int)sizeY / 10, sizeY - ((int)sizeY / 10));

			// if it's next to something else
			if ((placement != 0 || grid [x, y] != 0) && grass.Count > 0) {
				// get position from putAdjacent
				Vector2 pos = putAdjacent (grass);
				x = (int)pos.x;
				y = (int)pos.y;
			}

			// set as grass
			grid [x, y] = 1;
			// add to grass list to be able to put adjacent
			grass.Add (new Vector2 (x, y));

		}

		generateProgress += 10;
		generateProgressText = "Growing mountains";

		// build initial mountains chains
		for (int a = 0; a < mountainsNumber; a++) {
			// calculate random X
			int x = randomSeed.Next((int)sizeX / 10, sizeX - ((int)sizeX / 10));
			// calculate random Y
			int y = randomSeed.Next((int)sizeY / 10, sizeY - ((int)sizeY / 10));
			// number of turns the mountain chain will take during it's course
			int directions = randomSeed.Next(15, 30);
			// for each turn
			for (int k = 0; k < directions; k++) {
				// choose a direction
				Vector2 dir = new Vector2 (randomSeed.Next(-1, 1), randomSeed.Next(-1, 1));
				// number of steps in this direction
				int steps = randomSeed.Next(5, 10);
				// move for steps times
				for (int j = 0; j < steps; j++) {
					// recalculate the direction if it's the center
					if (dir.x == 0 && dir.y == 0) {
						dir = new Vector2 (randomSeed.Next(-1, 1), randomSeed.Next(-1, 1));
					}
					// apply to current center
					x += (int)dir.x;
					y += (int)dir.y;

					// if we're near the edge of the map don't add it
					if (x < 1 || y < 1 || x > sizeX - 1 || y > sizeY - 1) {
						break;
					}

					// add if it's not water
					if (grid [x, y] != SEA) {
						// highest value set for mountains
						grid [x, y] = highest;
						// add it to the top list
						tops.Add (new Vector2 (x, y));
					}

					// add the one next to it as well on x axis
					if (x < sizeX - 1 && grid [x + 1, y] != SEA) {
						grid [x + 1, y] = highest;
						tops.Add (new Vector2 (x+1, y));
					}
				}
			}
		}
		generateProgress += 10;

		// remove single cell. ex: on grass surrended by water or one water surrounded by grass
		for (int a = 1; a < sizeX - 1; a++) {
			for (int b = 1; b < sizeY - 1; b++) {
				// if it's water
				if (grid [a, b] == SEA) {
					// and have grass around in it's four direction
					if (hasAdjacent(grid, new Vector2(a,b), grid[a,b], FOUR_DIRECTION_MASK) == 0) {
						// replace by grass
						grid [a, b] = FLAT_GRASS;
					}
				// if it's anything else than water
				} else {
					// and has water around in all it's four direction
					if (hasAdjacent(grid, new Vector2(a,b), SEA, FOUR_DIRECTION_MASK) == 4) {
						// replace by water
						grid [a, b] = SEA;
					}
				}
			}
		}
		generateProgress += 10;
		generateProgressText = "Building continents";

		// smooth things (number of pass)
		int shouldSmooth = 2;
		// as long as we programmed another pass
		while (shouldSmooth >= 0) {
			// decrease remaining number of passes
			shouldSmooth -= 1;
			// loop on the grid
			for (int a = 1; a < sizeX - 1; a++) {
				for (int b = 1; b < sizeY - 1; b++) {
					// if it's water
					if (grid [a, b] == SEA) {
						// if it's surround by at least 3 grass
						int around = hasAdjacent(grid, new Vector2(a,b), SEA, FOUR_DIRECTION_MASK, SUPERIOR);
						if (around >= 3) {
							// make it grass
							grid [a, b] = FLAT_GRASS;
							// call for another smooth
							shouldSmooth += 1;
							
						}

					
						// if E and W are not water and the other 2 are 
						if ((hasAdjacent(grid, new Vector2(a,b), SEA, new int[] {0,0,0,1,0,1,0,0,0}) == 0 && around == 2) ||
							// or N and S are not water and the other 2 are
							(hasAdjacent(grid, new Vector2(a,b), SEA, new int[] {0,1,0,0,0,0,0,1,0}) == 0 && around == 2)) {
							// make it grass
							grid [a, b] = FLAT_GRASS;
							// call for another pass
							shouldSmooth += 1;
						}


					} else {

						// if it's a corner, replace with water
						if (a == 1 || b == 1 || a == sizeX - 2 || b == sizeY - 2) {
							grid [a, b] = SEA;
						}

						// if it's near a corner, make it not a straight line
						if (a == 2 || b == 2 || a == sizeX - 3 || b == sizeY - 3) {
							// random chance of replacing by water
							int shouldFragment = randomSeed.Next(0, 10);
							if (shouldFragment > 5) {
								grid [a, b] = SEA;
							}
						}
					}
				}
			}
		}
		generateProgress += 10;
		generateProgressText = "Smoothing mountains";
		// smooth mountains
		// loop through highest to lowest peak
		for (int i = highest; i > 1; i--) {
			// loop through the grid
			for (int x = 1; x < sizeX - 1; x++) {
				for (int y = 1; y < sizeY - 1; y++) {
					// if the tile if the height we're currently looping in
					if (grid [x, y] == i) {
						// loop through adjacent tiles
						for (int a = x - 1; a < x + 2; a++) {
							for (int b = y - 1; b < y + 2; b++) {
								// if adjacent tiles are lowers, put them same height
								if (grid [a, b] < grid [x, y] && grid [a, b] != SEA) {
									grid [a, b] = grid [x, y] - 1;
								}
							}
						}
					}
				}
			}
		}
		generateProgress += 10;
		generateProgressText = "Spreading Deserts";

		// deserts numbers
		int deserts = 0;
		// calculate sand tiles number based on desertPercent
		int sandTilesNb = (int) ((float)landTilesNb*((float)desertPercent/100));

		List<Vector2> desertTiles = new List<Vector2> ();

		// place deserts
		while (deserts < desertNumber) {
			// calculate random x
			int x = randomSeed.Next(3, sizeX);
			// calculate random y
			int y = randomSeed.Next(3, sizeY);
			// only if it's low level grass
			if (grid[x,y] == FLAT_GRASS && hasAdjacent (grid, new Vector2(x,y), SEA, NINE_POSITIONS_MASK, EQUAL) == 0) {
				// add it to desert list
				desertsSeeds.Add (new Vector2 (x, y));
				desertTiles.Add (new Vector2 (x, y));
				deserts++;
			}
		}

		// place desert tile
		for (int i = 0; i < sandTilesNb-desertsSeeds.Count; i++) {
			// find adjacent position next to already existing desert tile
			Vector2 pos = putAdjacent (desertsSeeds, highest);
			int x = (int)pos.x;
			int y = (int)pos.y;

			int iter = 0;

			while (hasAdjacent (grid, new Vector2 (x, y), SEA, NINE_POSITIONS_MASK, EQUAL) != 0 && iter < 10) {
				pos = putAdjacent (desertsSeeds, highest);
				x = (int)pos.x;
				y = (int)pos.y;
				iter++;
			}

			if (hasAdjacent (grid, new Vector2 (x, y), SEA, NINE_POSITIONS_MASK, EQUAL) == 0) {
				// add it as a desert
				grid [x, y] = DESERT;
				// add it to the list of desert seeds
				desertsSeeds.Add (new Vector2 (x, y));
				desertTiles.Add (new Vector2 (x, y));
			}

		}
		generateProgress += 10;

		for (int a = 0; a < sizeX; a++) {
			for (int b = 0; b < sizeY; b++) {
				// if it's grass
				if (grid [a, b] == 1) {
					// number of deserts in four directions
					int desertAround = hasAdjacent(grid, new Vector2(a,b), DESERT, FOUR_DIRECTION_MASK);

					// if there is at least 3 desert around
					if (desertAround >= 3) {
						// make it desert
						grid [a, b] = DESERT;
					}
				}
			}
		}

		int palmTreeNb = (int)((float)sandTilesNb * 0.015);
		for (int i = 0; i < palmTreeNb; i++) {

			int pos = randomSeed.Next(0, desertTiles.Count);

			while(objects[(int)desertTiles[pos].x, (int)desertTiles[pos].y] != 0) {

				desertTiles.RemoveAt (pos);
				pos = randomSeed.Next(0, desertTiles.Count);
			}

			objects [(int)desertTiles [pos].x, (int)desertTiles [pos].y] = PALM_TREE;
		}
		generateProgress += 10;


		// remove patterns
		// as long as through will loop
		bool keepItSmooth = true;
		// emergency exist flag
		int iteration = -1;
		while (keepItSmooth && iteration < 10) {
			keepItSmooth = false;
			iteration += 1;
			// loop through grid
			for (int x = 2; x < sizeX - 4; x++) {
				for (int y = 1; y < sizeY - 3; y++) {

					// if it's not desert
					if (grid [x, y] < DESERT) {
						// if pattern 101 => 000 
						if (grid [x + 1, y] == grid [x - 1, y] && Mathf.Abs (grid [x, y] - grid [x - 1, y]) == 1 &&
							grid [x + 1, y] > FLAT_GRASS && grid [x, y] > FLAT_GRASS) {
							int value = grid [x + 1, y];
							if (grid [x, y] > value) {
								value = grid [x, y];
							}
							grid [x - 1, y] = value;
							grid [x + 1, y] = value;
							grid [x, y] = value;
							keepItSmooth = true;
						}
						// if pattern 0110 => 0000
						if (grid [x - 2, y] == grid [x + 1, y] && Mathf.Abs (grid [x, y] - grid [x + 1, y]) == 1 &&
							grid [x + 1, y] > FLAT_GRASS && grid [x, y] > FLAT_GRASS && grid [x - 1, y] == grid [x, y]) {
							int value = grid [x + 1, y];
							if (grid [x, y] > value) {
								value = grid [x, y];
							}
							grid [x, y] = value;
							grid [x + 1, y] = value;
							grid [x - 2, y] = value;
							grid [x + 1, y] = value;
							keepItSmooth = true;
						}
						// if pattern 1122 => 2222
						if (grid [x - 1, y] == grid [x, y] && Mathf.Abs (grid [x, y] - grid [x + 1, y]) == 1 &&
							grid [x + 1, y] > FLAT_GRASS && grid [x, y] > FLAT_GRASS && grid [x + 1, y] == grid [x + 2, y]) {
							int value = grid [x + 1, y];
							if (grid [x, y] > value) {
								value = grid [x, y];
							}
							grid [x, y] = value;
							grid [x - 1, y] = value;
							grid [x + 1, y] = value;
							grid [x + 2, y] = value;
							keepItSmooth = true;
						}

						// remove only one tile down or up from "elev" z-index in a square
						if (grid [x, y] != 0) {
							for (int elev = -highest; elev < highest; elev++) {
								if (elev == 0 || (grid [x, y] + elev) < FLAT_GRASS) {
									continue;
								}

								if (hasAdjacent(grid, new Vector2(x,y), grid [x, y] + elev) >= 7) {
									grid [x, y] += elev;
									break;
								}
							}
						}
					}
				}
			}
		}
		generateProgress += 10;
		generateProgressText = "Drawing rivers";
		// create rivers
		for (int i = 0; i < riversNb; i++) {
			// find a random mountain peak
			Vector2 pos = tops [randomSeed.Next(0,tops.Count)];
			int iter = 0;

			// as long as there is water nearby or a river next to it
			while ((hasAdjacent(grid, new Vector2(pos.x,pos.y), SEA, FOUR_DIRECTION_MASK) >= 1 ||
					hasAdjacent(grid, new Vector2(pos.x,pos.y), DESERT, HEIGHT_DIRECTION_MASK) >= 1 ||
					hasAdjacentInList(rivers, new Vector2(pos.x,pos.y), FOUR_DIRECTION_MASK) > 0)
					&& iter < 10) {

				// look for a new position
				pos = tops [randomSeed.Next(0,tops.Count)];
				// emergency exit flag
				iter++;
				
			}

			int x = (int) pos.x;
			int y = (int) pos.y;

			// start with a lake 3x3
			for (int g = x - 1; g < x + 2; g++) {
				for (int h = y - 1; h < y + 2; h++) {
					if (grid [g, h] != SEA &&
						hasAdjacent(grid, new Vector2(g,h), DESERT, NINE_POSITIONS_MASK) == 0) {
						rivers.Add (new Vector2 (g, h));
					}
				}
			}

			// last direction vector used initiliased with center.
			Vector2 lastVector = new Vector2 (0, 0); 
			// possible direction for water flowing
			Vector2[] dirs = { new Vector2 (-1, 0), new Vector2 (1, 0), new Vector2 (0, 1), new Vector2 (0, -1) };

			// get a random direction
			int initR = randomSeed.Next(0, dirs.Length);

			// find position from direction.
			x += (int) dirs [initR].x * 2;
			y += (int) dirs [initR].y * 2;

			// keep looking until you find a spot
			bool keepLooking = true;

			// keep looking for place to add river until it's sea or we couldn't find a spot.
			while (grid [x, y] > SEA &&
				hasAdjacent(grid, new Vector2(x,y), DESERT, NINE_POSITIONS_MASK) == 0 && keepLooking) {
				// add to river tiles
				rivers.Add (new Vector2 (x, y));
				bool found = false;

				// as long as we didn't find a spot
				while (!found) {
					// last vector used to find same height in another tile
					Vector2 lastVectorSame = new Vector2(0,0);

					// start with a direction
					int startIndex = randomSeed.Next(0, dirs.Length);
					// loop through each direction
					for (int j = 0; j < dirs.Length; j++) {
						// if we reach the end of the array of directions start at the start
						if (startIndex >= dirs.Length) {
							startIndex = 0;
						}

						// if it's the same as where we come from, try again
						if (dirs [startIndex].x == -lastVector.x && dirs[startIndex].y == -lastVector.y) {
							startIndex++;
							continue;
						}

						// if there is already a river there, try again
						if (rivers.Contains (new Vector2 (x + (int)dirs [startIndex].x, y + (int)dirs [startIndex].y))) {
							startIndex++;
							continue;
						}

						// find lower ground to go
						if (grid [x + (int) dirs [startIndex].x, y + (int) dirs [startIndex].y] < grid [x, y]) {
							lastVector = dirs [startIndex];
							// lower ground found
							x += (int)dirs [startIndex].x;
							y += (int)dirs [startIndex].y;
							found = true;
							break;
						}

						// find same height ground to go
						if (grid [x + (int) dirs [startIndex].x, y + (int) dirs [startIndex].y] == grid [x, y] &&
							(int) (lastVectorSame.x  + lastVectorSame.y) == 0) {
							lastVectorSame = dirs [startIndex];
						}

						startIndex += 1;
					}

					// if we didn't find lower but found at the same height
					if (found == false && (int) (lastVectorSame.x  + lastVectorSame.y)  != 0) {
						// last vector is the one used for same height
						lastVector = lastVectorSame;
						x += (int)lastVectorSame.x;
						y += (int)lastVectorSame.y;
						found = true;

					// if we didn't find anything
					} else if (found == false) {
						keepLooking = false;
						break;
					}
				}
			
			}

			// clear the top from the list, we don't want a second river there
			tops.RemoveAt (i);
		}

		generateProgress += 10;
		generateProgressText = "Planting trees";
		// calculate forest tiles number
		int forestTilesNb = (int) ((float)landTilesNb*((float)forestPercent/100));
		// number of forest
		int forests = 0;
		// place forests seeds
		while (forests < (int) ((float)forestTilesNb*0.05)) {
			// find a random spot
			int x = randomSeed.Next(3, sizeX);
			int y = randomSeed.Next(3, sizeY);
			// if it's not water
			if (grid[x,y] != SEA &&
				// if there is no desert adjacent to it
				hasAdjacent(grid, new Vector2(x,y), DESERT, NINE_POSITIONS_MASK, EQUAL) == 0
				// and not tree there already
				&& objects[x,y] == 0 
				// and no rivers here
				&& !rivers.Contains(new Vector2(x,y))) {

				// get a random tree number
				objects [x, y] = (randomSeed.Next(0, (int)(trees.Length/2))*2)+1;
				// add it to the seeds list
				forestsSeeds.Add (new Vector2 (x, y));
				forests++;
			}
		}

		// place forests tile
		for (int i = 0; i < forestTilesNb-forestsSeeds.Count; i++) {
			// put it adjacent to another forest
			Vector2 pos = putAdjacent (forestsSeeds, 0, true);
			int x = (int)pos.x;
			int y = (int)pos.y;

			int index = 0;

			// find the kind of tree by what is around
			for (int a = x-1 ; a < x+2; a++) {
				for (int b = y-1; b < y+2; b++ ) {
					if (a == 0 && b == 0) {
						continue;
					}
					if (objects[a,b] != 0) {
						// add the tree around
						index = (int)((objects [a, b]-1) / 2);
						break;
					}
				}
			}
			// take a random tree for the tree family found
//			int index = treesAround.ToList().IndexOf(treesAround.Max());
			objects [x, y] = randomSeed.Next((index*2), (index*2)+2)+1;
			forestsSeeds.Add (new Vector2 (x, y));
		}
		generateProgress += 10;
		generateProgressText = "Finalising...";
	}

	// find if there is adjacent tiles in a list
	// inList: list to look in
	// pos: position of the subject
	// mask: mask to apply
	int hasAdjacentInList(List<Vector2> inList, Vector2 pos, int[] mask) {
		// if there is no mask. take the 8 directions by default
		mask = mask ?? HEIGHT_DIRECTION_MASK;

		// number of occurences found
		int found = 0;

		// iterate through the mask
		for (int i = 0; i < mask.Length; i++) {
			// if we're looking for something there
			if (mask [i] == 1) {
				// check if there is a vector which hve the same value in list
				if (inList.Contains(new Vector2(pos.x+translations[i].x, pos.y+translations[i].y))) {
					found++;
				}
			}
		}

		// return number of occurences found
		return found;
	}

	// find adjacents tiles in grid
	// inArray: grid to look in
	// pos: position of the subject
	// val: value to look for
	// mask: mask to apply
	// operand: are we looking for ==, >, >=, <, <= ?
	int hasAdjacent(int[,] inArray, Vector2 pos, int val, int[] mask = null, int operand = EQUAL) {
		// if no mask, look through all height directions
		mask = mask ?? HEIGHT_DIRECTION_MASK;
		// number of occurences found
		int found = 0;

		// loop through the mask
		for (int i = 0; i < mask.Length; i++) {
			// if we're looking for this position
			if (mask[i] == 1) {
				// check which operand we're looking with
				switch (operand) {
				// <= 
				case INFERIOR_EQUAL:
					// check if the value is <= to what we're looking for
					if (inArray [(int)pos.x + (int)translations [i].x, (int)pos.y + (int)translations [i].y] <= val) {
						found++;
					}
					break;
				// <
				case INFERIOR:
					// check if the value is < to what we're looking for
					if (inArray [(int)pos.x + (int)translations [i].x, (int)pos.y + (int)translations [i].y] < val) {
						found++;
					}
					break;
				// ==
				case EQUAL:
					// check if the value is == to what we're looking for
					if (inArray [(int)pos.x + (int)translations [i].x, (int)pos.y + (int)translations [i].y] == val) {
						found++;
					}
					break;
				// >
				case SUPERIOR:
					// check if the value is > to what we're looking for
					if (inArray [(int)pos.x + (int)translations [i].x, (int)pos.y + (int)translations [i].y] > val) {
						found++;
					}
					break;
					// >=
				case SUPERIOR_EQUAL:
					// check if the value is >= to what we're looking for
					if (inArray [(int)pos.x + (int)translations [i].x, (int)pos.y + (int)translations [i].y] >= val) {
						found++;
					}
					break;
				}
			}
		}

		// return number of occurences found
		return found;

	}

	// put a tile next to a another specific tiles
	// tiles: list of tiles to look into
	// limit: tile value limit for search
	// isTree: are we placing forest?
	Vector2 putAdjacent(List<Vector2> tiles, int limit = 0, bool isTrees = false) {
		// take a random index in the list
		Random randomSeed = new Random();
		int pos = randomSeed.Next(0, tiles.Count);

		// four adjacents vectors (N,W,S,E)
		Vector2[] adjacents = {new Vector2 (0, 1),
			new Vector2 (1, 0),
			new Vector2 (0, -1),
			new Vector2 (-1, 0)
		};

		// initial in the direction vectors
		int initialIndex = randomSeed.Next(0, adjacents.Length);
		// index in the direction array 
		int index = initialIndex;

		// get coord from tile we found
		int x = (int)(tiles [pos].x + adjacents [index].x);
		int y = (int)(tiles [pos].y + adjacents [index].y);
		int iter = 0;
		// keep looking as long as
		while (
			// if it's not trees
			(!isTrees?
				// the tile is above the limit
				(grid[x,y] > limit):
				// if it's trees
				// if the tile is water, or desert
				(grid[x,y] == SEA ||
					// or if there is a river adjacent to it
					hasAdjacentInList(rivers, new Vector2(x,y), NINE_POSITIONS_MASK) > 0 ||
					// or if there is a desert adjacent to it
					hasAdjacent(grid, new Vector2(x,y), DESERT, NINE_POSITIONS_MASK, EQUAL) > 0 ||
					// or if there is already a tree there
					objects[x,y] != 0
				)
			// or if we are at the edge of the map
			) || x < 3 || y < 3 || x > sizeX-5 || y > sizeY-5) {

			// calculate coordinates
			x = (int)(tiles [pos].x + adjacents [index].x);
			y = (int)(tiles [pos].y + adjacents [index].y);
			// if we have the same direction we had initially (meaning we parsed them all)
			if (index == initialIndex && iter != 0) {
				// remove the current result
				tiles.RemoveAt (pos);
				// find adjacent from another position
				Vector2 res =  putAdjacent (tiles, limit, isTrees);
				x = (int) res.x;
				y = (int) res.y;
				break;
			}

			// increase the index ( new direction)
			index += 1;
			// if we are over the length of the list, start again from 0
			if (index >= adjacents.Length) {
				index = 0;
			}
			iter++;
		}

		// return new tile position
		return new Vector2 (x, y);
	}


	// fill the grid with correct game objects depending of their surroundings
	public void Draw() {
		// y drawing coordinate
		int drawY 	= 0;
		// y drawing coordinate
		int drawX 	= 0;
		// current zindex
		int zIndex 	= 0;
		// grass number of tiles (debug)
		int grassNb = 0;
		// half vertical isometric decal
		float halfDecalY = (decalY / 2);

		// loop through the grid
		for (int y = 0; y < sizeY; y++) {

			for (int x = 0; x < sizeX; x++) {

				// calculate drawing coordinates
				drawX 	= -y + x;
				drawY 	= x + y;
				// calculate zindex (each cell have 3 zindex level)
				zIndex 	= (x + y)*3;
				// calculate drawing coordinates
				float displayX = (decalX * drawX);
				float displayY = (decalY * drawY);
				// calculate decaling for trees
				float treeDecalY = 0f;
				// current tile
				GameObject tile = null;
				// current stack
				GameObject[] fillerStack = stackDirt;
				// if it filled with sea
				bool isFillerSea = false;
				// current height
				float height = 0;

				// if it's a river and not sea
				if (rivers.Contains (new Vector2 (x, y)) && grid[x,y] != SEA) {

					// if W tile is lower
					if (grid [x - 1, y] < grid [x, y] && grid[x-1,y] != DESERT) {
						// stack is waterfall horizontal
						fillerStack = stackWaterfallHor;
					}

					// if N tile is lower
					if (grid [x, y - 1] < grid [x, y] && grid[x,y-1] != DESERT) {
						// stack is waterfall vertical
						fillerStack = stackWaterfallVer;
					}

					// river corner E
					if (hasAdjacentInList(rivers, new Vector2(x,y), SE_CORNER) == 3 &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,1,0,1,0,0,0,0,0}) == 0) 
					{
						fillerStack = stackWaterfallVer;
						tile = riverCornerE;
					}
					// river corner N
					if (hasAdjacentInList(rivers, new Vector2(x,y), NE_CORNER) == 3 &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,0,0,1,0,0,0,1,0}) == 0 &&
						tile == null) {

						fillerStack = stackDirt;
						tile = riverCornerN;
					}
					// river corner S
					if (hasAdjacentInList(rivers, new Vector2(x,y), SW_CORNER) == 3 &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,1,0,0,0,1,0,0,0}) == 0 &&
						tile == null) {

						tile = riverCornerS;
					}
					// river corner W
					if (hasAdjacentInList(rivers, new Vector2(x,y), NW_CORNER) == 3 &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,0,0,0,0,1,0,1,0}) == 0 &&
						tile == null) {

						fillerStack = stackWaterfallHor;
						tile = riverCornerW;
					}

					// river turn E
					if ((rivers.Contains (new Vector2 (x + 1, y)) || grid[x+1,y] == SEA) &&
						tile == null &&
						(rivers.Contains (new Vector2 (x, y - 1))  || grid[x, y-1] == SEA) &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,1,0,1,0,0,0,0,0}) == 0) {
						fillerStack = stackWaterfallVer;
						tile = riverTurnE;
					}
					// river turn N
					if ((rivers.Contains (new Vector2 (x + 1, y)) || grid[x+1,y] == SEA ) &&
						tile == null &&
						(rivers.Contains (new Vector2 (x, y + 1)) || grid[x,y+1] == SEA ) &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,0,0,1,0,0,0,1,0}) == 0) {
						fillerStack = stackDirt;
						tile = riverTurnN;
					}
					// river turn S
					if ((rivers.Contains (new Vector2 (x - 1, y)) || grid[x-1,y] == SEA) &&
						tile == null &&
						(rivers.Contains (new Vector2 (x, y - 1)) || grid[x,y-1] == SEA) &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,1,0,0,0,1,0,0,0}) == 0) {
						tile = riverTurnS;
					}
					// river turn W
					if ((rivers.Contains (new Vector2 (x - 1, y)) || grid[x-1,y] == SEA) &&
						tile == null &&
						(rivers.Contains (new Vector2 (x, y + 1)) || grid[x,y+1] == SEA) &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,0,0,0,0,1,0,1,0}) == 0) {
						fillerStack = stackWaterfallHor;
						tile = riverTurnW;
					}

					// river wall SW
					if (rivers.Contains (new Vector2 (x + 1, y)) && !rivers.Contains (new Vector2 (x - 1, y)) &&
						tile == null && grid[x-1,y] != SEA) {
						isFillerSea = true;
						tile = riverSW;
					}

					// river wall NE
					if (rivers.Contains (new Vector2 (x - 1, y)) && !rivers.Contains (new Vector2 (x + 1, y)) &&
						tile == null && grid[x+1,y] != SEA) {
						isFillerSea = true;
						tile = riverNE;
					}


					// river wall NW
					if (rivers.Contains (new Vector2 (x, y -1)) && !rivers.Contains (new Vector2 (x, y + 1)) &&
						tile == null && grid[x,y+1] != SEA) {
						isFillerSea = true;
						tile = riverNW;
					}

					// river wall SE
					if (rivers.Contains (new Vector2 (x, y +1)) && !rivers.Contains (new Vector2 (x, y - 1)) &&
						tile == null && grid[x,y-1] != SEA) {
						isFillerSea = true;
						tile = riverSE;
					}

					// river horizontal
					if ((rivers.Contains (new Vector2 (x + 1, y)) || grid[x+1,y] == SEA) && tile == null &&
						(rivers.Contains (new Vector2 (x - 1, y )) || grid[x-1,y] == SEA) &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,1,0,0,0,0,0,1,0}) == 0) {
						tile = riverHor;
						fillerStack = stackWaterfallHor;
						// if it's not straight, use river ramps
						if (grid [x - 1, y] > grid [x, y]) {
							tile = smallRiverUpSW;
						}
						if (grid [x - 1, y] > grid [x, y]+1) {
							tile = bigRiverUpSW;
						}
						if (grid [x + 1, y] > grid [x, y]) {
							tile = smallRiverUpNE;
						}
						if (grid [x + 1, y] > grid [x, y]+1) {
							tile = bigRiverUpNE;
						}
					}
					// river vertical
					if ((rivers.Contains (new Vector2 (x, y -1)) || grid[x,y-1] == SEA) && tile == null &&
						(rivers.Contains (new Vector2 (x, y +1)) || grid[x, y+1] == SEA) &&
						hasAdjacentInList(rivers, new Vector2(x,y), new int[] {0,0,0,1,0,1,0,0,0}) == 0) {
						tile = riverVer;
						fillerStack = stackWaterfallVer;
						// if it's not straight, use river ramp
						if (grid [x, y - 1] > grid [x, y]) {
							tile = smallRiverUpSE;
						}
						if (grid [x, y - 1] > grid [x, y]+1) {
							tile = bigRiverUpSE;
						}
						if (grid [x, y + 1] > grid [x, y]) {
							tile = smallRiverUpNW;
						}
						if (grid [x, y + 1] > grid [x, y]+1) {
							tile = bigRiverUpNW;
						}
					}

					// river north inner corner
					if (!rivers.Contains (new Vector2 (x + 1, y + 1)) && grid[x+1,y+1] != SEA && tile == null) {
						isFillerSea = true;
						tile = riverInnerCornerN;
					}

					// river south inner corner
					if (!rivers.Contains (new Vector2 (x - 1, y - 1)) && grid[x-1,y-1] != SEA  && tile == null) {
						isFillerSea = true;
						tile = riverInnerCornerS;
					}

					// river west inner corner
					if (!rivers.Contains (new Vector2 (x - 1, y + 1)) && grid[x-1,y+1] != SEA && tile == null) {
						isFillerSea = true;
						tile = riverInnerCornerW;
					}
						
					// river west inner corner
					if (!rivers.Contains (new Vector2 (x + 1, y - 1)) && grid[x+1,y-1] != SEA && tile == null) {
						isFillerSea = true;
						tile = riverInnerCornerE;
					}

					// if we didn't found anything, it's sea
					if (tile == null) {
						tile = flatSea;
					}
				}

				// if we haven't found anything
				if (tile == null) {
					// if it's above see level
					if (grid [x, y] > SEA && grid[x,y] <= highest) {
						// NE higher grass
						if (grid [x + 1, y] > grid [x, y] && grid [x + 1, y] < grid [x, y] + 3 && tile == null &&
							grid [x + 1, y] < DESERT) {
							tile = grid [x + 1, y] - grid [x, y] == 1 ? smallGrassUpNE : bigGrassUpNE;
							treeDecalY += halfDecalY;
						}

						// SW higher grass
						if (grid [x - 1, y] > grid [x, y] && grid [x - 1, y] < grid [x, y] + 3 && tile == null &&
							grid [x - 1, y] < DESERT) {
							tile = grid [x - 1, y] - grid [x, y] == 1 ? smallGrassUpSW : bigGrassUpSW;
							treeDecalY += halfDecalY;
						}

						// NW higher grass
						if (grid [x, y + 1] > grid [x, y] && grid [x, y + 1] < grid [x, y] + 3 && tile == null &&
							grid [x, y + 1] < DESERT) {
							tile = grid [x, y + 1] - grid [x, y] == 1 ? smallGrassUpNW : bigGrassUpNW;
							treeDecalY += halfDecalY;
						}

						// SE higher grass
						if (grid [x, y - 1] > grid [x, y] && grid [x, y - 1] < grid [x, y] + 3 && tile == null &&
							grid [x, y - 1] < DESERT) {
							tile = grid [x, y - 1] - grid [x, y] == 1 ? smallGrassUpSE : bigGrassUpSE;
							treeDecalY += halfDecalY;
						}
						grassNb += 1;

						// if we haven'T found anything and it's lower level grass (height = 1)
						if (tile == null && grid[x,y] == FLAT_GRASS) {
							// grass adjacent to DESERT NORTH
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,0,0,1,0,0,0,1,0}) == 2) {
								tile = grassDesertN;
							}
							// grass adjacent toDESERT SOUTH
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,1,0,0,0,1,0,0,0}) == 2) {
								tile = grassDesertS;
							}
							// grass adjacent to WEST DESERT
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,0,0,0,0,1,0,1,0}) == 2) {
								tile = grassDesertE;
							}
							// grass adjacent to EAST DESERT
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,1,0,1,0,0,0,0,0}) == 2) {
								tile = grassDesertW;
							}
						}

						// if it's grass and we still ahven't found anything
						if (grid[x,y] == FLAT_GRASS && tile == null) {
							// grass adjacent to desert SW
							if (grid [x - 1, y] == DESERT) {
								tile = grassDesertSE;

							} else if (grid [x + 1, y] == DESERT) {
								// grass adjacent to desert NE
								tile = grassDesertNW;

							} else if (grid [x, y - 1] == DESERT) {
								// grass adjacent to desert SE
								tile = grassDesertSW;

							} else if (grid [x, y + 1] == DESERT) {
								// grass adjacent to desert NW
								tile = grassDesertNE;

							}

							// if it's none of the above
							if (tile == null) {
								if (grid [x + 1, y + 1] == DESERT) {
									// grass adjacent to desert inner N
									tile = grassInnerDesertN;
								}else if (grid [x - 1, y - 1] == DESERT) {
									// grass adjacent to desert inner S
									tile = grassInnerDesertS;
								} else if (grid [x - 1, y + 1] == DESERT) {
									// grass adjacent to desert inner W
									tile = grassInnerDesertW;
								} else if (grid [x + 1, y - 1] == DESERT) {
									// grass adjacent to desert inner E
									tile = grassInnerDesertE;
								}
							}
						}

						// if it'S still nothing, it's plain grass
						if (tile == null) {
							tile = flatGrass;
						}
					
					// if it's desert
					} else if (grid[x,y] == DESERT){
						tile = sand;

					// if it's none of the above and doesn't have river next to it
					} else if(hasAdjacentInList(rivers, new Vector2(x,y), FOUR_DIRECTION_MASK) == 0) {
						// and we're not near the edge of the board
						if (x > 0 && x < sizeX - 1 && y > 0 && y < sizeY - 1) {
							// beach NORTH
							if (grid [x - 1, y] == FLAT_GRASS && grid [x, y - 1] == FLAT_GRASS) {
								tile = beachN;
								
							}
							// beach to DESERT NORTH
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,0,0,1,0,0,0,1,0}) == 2) {
								tile = beachDesertN;
							}
							// beach SOUTH
							if (hasAdjacent(grid, new Vector2(x,y), FLAT_GRASS, new int[] {0,1,0,0,0,1,0,0,0}) == 2){
								tile = beachS;

							}
							// beach to DESERT SOUTH
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,1,0,0,0,1,0,0,0}) == 2) {
								tile = beachDesertS;
							}
							// beach WEST
							if (hasAdjacent(grid, new Vector2(x,y), FLAT_GRASS, new int[] {0,0,0,0,0,1,0,1,0}) == 2){
								tile = beachW;

							}
							// beach to WEST DESERT
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,0,0,0,0,1,0,1,0}) == 2) {
								tile = beachDesertE;
							}
							// beach EAST
							if (hasAdjacent(grid, new Vector2(x,y), FLAT_GRASS, new int[] {0,1,0,1,0,0,0,0,0}) == 2){
								tile = beachE;

							}
							// beach to EAST DESERT
							if (hasAdjacent(grid, new Vector2(x,y), DESERT, new int[] {0,1,0,1,0,0,0,0,0}) == 2){
								tile = beachDesertW;
							}

							// if we still haven't found anything
							if (tile == null) {
								// beach SW
								if (grid [x - 1, y] == FLAT_GRASS || grid [x - 1, y] == DESERT) {
									tile = beachSW;

								} else if (grid [x + 1, y] == FLAT_GRASS || grid [x + 1, y] == DESERT) {
									// beach NE
									tile = beachNE;

								} else if (grid [x, y - 1] == FLAT_GRASS || grid [x, y - 1] == DESERT) {
									// beach SE
									tile = beachSE;

								} else if (grid [x, y + 1] == FLAT_GRASS || grid [x, y + 1] == DESERT) {
									// beach NW
									tile = beachNW;

								}
							}

							// if we still haven't found anything
							if (tile == null) {
								if (grid [x + 1, y + 1] == FLAT_GRASS || grid [x + 1, y + 1] == DESERT) {
									// beach inner N
									tile = beachInnerN;
								} else if (grid [x - 1, y + 1] == FLAT_GRASS || grid [x - 1, y + 1] == DESERT) {
									// beach inner W
									tile = beachInnerW;
								} else if (grid [x + 1, y - 1] == FLAT_GRASS || grid [x + 1, y - 1] == DESERT) {
									// beach inner E
									tile = beachInnerE;
								}
							}
						}

					}

				}

				if (tile == null && objects [x, y] == 0) {
					tile = flatSea;
				}

				// display the tile
				bool display = true;
				// if we'not on the edge and
				if (y > 0 && x > 0 && 
					// south tiles is > center tile + 3
					grid[x,y-1] > grid[x,y]+3 &&
					// south west tile is > center tile +3
					grid[x-1,y-1] > grid[x,y]+3 &&
					// south tile is not desert
					grid[x,y-1] < DESERT &&
					// south west tile is not desert
					grid[x-1,y-1] < DESERT) {
					// it's hidden, don't display it
					display = false;
				}
				// if we have a tile and it's displayable
				if (display && tile != null) {
					// if we're higher than flat grass and lower than top
					if (grid [x, y] > FLAT_GRASS && grid[x,y] <= highest) {
						// had decalY depending on height
						height += (grid [x, y] - 1) * halfDecalY;
						// get the correct filler stack
						GameObject filler = fillerStack [grid [x, y] - 2];
						// if it's flat sea or the filler is flat sea
						if (tile == flatSea || isFillerSea == true) {
							// get the correct stack in the stackwater list
							filler = stackWater [grid [x, y] - 2];;
						}
						// if south is lower
						if ((grid[x,y-1] < grid[x,y] ||
							// or south is desert
							grid[x,y-1] == DESERT) ||
							// or west is lower
							(grid[x-1,y] < grid[x,y] ||
							// or west is desert
							grid[x-1,y] == DESERT)) {
							// set filler in instance
							instances[x,y,1] = filler;
							// set filler position in instances
							instancesPositions[x,y,1] = new Vector3 (displayX, displayY + height - (grid[x,y]-2)*halfDecalY, zIndex + 2);
						}
					}

					// set tile in instance
					instances[x,y,0] = tile;

					// if it's not SW corner
					if (x != 0 || y != 0) {
						// set the position of the instance
						instancesPositions[x,y,0] = new Vector3 (displayX, displayY + height, zIndex+1);
					}
				}

				// if we have an object there
				if (objects[x,y] != 0) {
					if (objects [x, y] < PALM_TREE) {
						// find corresponding tree
						instances [x, y, 2] = trees [objects [x, y]-1];
					} else {
						instances [x, y, 2] = palmTree;
					}
					// set it's position
					instancesPositions[x,y,2] = new Vector3 (displayX, displayY + height+treeDecalY+((height+1)*halfDecalY), zIndex);
				}
			}
		}

		// clear the grass list
		grass.Clear ();
	}


	// Use this for initialization
	public void Initialize () {
		// initialise the grid
		grid = new int [sizeX, sizeY];
		// initialise the object grid
		objects = new int [sizeX, sizeY];
		generateProgress = 0;

		// start with water everywhere
		for (int y = 0; y < sizeY; y++) {

			for (int x = 0; x < sizeX; x++) {

				grid [x, y] = 0;
			}
		}
		// init the instances
		instances = new GameObject[sizeX, sizeY, 3];
		// init the instances positions
		instancesPositions = new Vector3[sizeX, sizeY, 3];
		// init the instances indexes in pool
		instanceIndexes = new int[sizeX, sizeY, 3];
		// init the instances sprites
		instanceSprites = new Sprite[sizeX, sizeY, 3];
		generateWorld   = new Thread(GenerateWorld);
		// generate the new world
		generateWorld.Start();
	}

	public void SetUpSprites() {
		// correspond the tile to the grid
		Draw ();

		grass.Clear ();
		tops.Clear ();
		desertsSeeds.Clear ();
		forestsSeeds.Clear ();

		// initiaise the sprites and indexes
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				for (int k = 0; k < 3; k++) {
					// set to -1, there is no indexes in pool currently
					instanceIndexes [i, j, k] = -1;
					// get the sprite of the tile at those coordniates or null if there is nothing
					instanceSprites [i, j, k] = instances[i,j,k] == null? null : instances[i,j,k].GetComponent<SpriteRenderer>().sprite;
				}
			}
		}
	}
}
