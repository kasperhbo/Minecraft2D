using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class World : MonoBehaviour
{
    //Vectors to check
    Vector2 nCheck = Vector2.up;
    Vector2 eCheck = Vector2.right;
    Vector2 sCheck = Vector2.down;
    Vector2 wCheck = Vector2.left;

    // int[
    //     2 = 1
    //     8 = 2,
    //     10 = 3,
    //     11 = 4, 
    //     16 = 5, 
    //     18 = 6,
    //     22 = 7, 
    //     24 = 8, 
    //     26 = 9,
    //     27 = 10,
    //     30 = 11, 
    //     31 = 12, 
    //     64 = 13, 
    //     66 = 14, 
    //     72 = 15, 
    //     74 = 16, 
    //     75 = 17, 
    //     80 = 18, 
    //     82 = 19, 
    //     86 = 20,
    //     88 = 21, 
    //     90 = 22,
    //     91 = 23,
    //     94 = 24,
    //     95 = 25, 
    //     104 = 26,
    //     106 = 27, 
    //     107 = 28, 
    //     120 = 29,
    //     122 = 30, 
    //     123 = 31,
    //     126 = 32,
    //     127 = 33, 
    //     208 = 34, 
    //     210 = 35, 
    //     214 = 36, 
    //     216 = 37, 
    //     218 = 38, 
    //     219 = 39, 
    //     222 = 40, 
    //     223 = 41, 
    //     248 = 42, 
    //     250 = 43, 
    //     251 = 44, 
    //     254 = 45, 
    //     255 = 46, 
    //     0 = 47
    // ];


    public Tile testTile;

    public GameObject txtComp;
    private List<GameObject> textilst = new List<GameObject>();

    //Block Types
    [SerializeField] private List<BlockType> blockTypes = new List<BlockType>();

    public Tilemap tileMap;
    
    //World Data
    private Dictionary<Vector2, Voxel> voxelsInWorld = new Dictionary<Vector2, Voxel>();

    private void Start()
    {
        CreateWaterWorldData();
        // SpawnWorld();
    }
#region old


    // private void SpawnWorld()
    // {
    //     for (int i = 0; i < voxelsInWorld.Count; i++)
    //     {
    //         var voxel = voxelsInWorld[i];
    //         
    //         int total = 0;
    //         //do checks
    //         int n = 0;
    //         
    //         //n * top
    //         n = 1;
    //         Voxel voxToCheckUp = null;
    //         foreach (var vox in voxelsInWorld)
    //         {
    //             if (vox.location == new Vector3Int(voxel.location.x, voxel.location.y + 1, 0))
    //             {
    //                 voxToCheckUp = vox;
    //                 //Debug.Log(voxToCheckUp.blockType.tileID);
    //             }
    //         }
    //
    //         if (voxToCheckUp != null)
    //         {
    //             var nNumber = n * voxToCheckUp.blockType.tileID;
    //             
    //             total += nNumber;
    //         }
    //         
    //         //n * left
    //         n = 2;
    //         Voxel voxToCheckLeft = null;
    //         foreach (var vox in voxelsInWorld)
    //         {
    //             if (vox.location == new Vector3Int(voxel.location.x - 1, voxel.location.y, 0))
    //             {
    //                 voxToCheckLeft = vox;
    //             }
    //         }
    //
    //         if (voxToCheckLeft != null)
    //         {
    //             var nNumber = n * voxToCheckLeft.blockType.tileID;
    //             total += nNumber;
    //         }
    //         
    //         //n * right
    //         n = 4;
    //         Voxel voxToCheckRight = null;
    //         foreach (var vox in voxelsInWorld)
    //         {
    //             if (vox.location == new Vector3Int(voxel.location.x+ 1, voxel.location.y , 0))
    //             {
    //                 voxToCheckRight = vox;
    //             }
    //         }
    //
    //         if (voxToCheckRight != null)
    //         {
    //             var nNumber = n * voxToCheckRight.blockType.tileID;
    //             total += nNumber;
    //         }
    //         
    //         //n * times bottom
    //         n = 8;      
    //         Voxel voxToCheckBot = null;
    //         foreach (var vox in voxelsInWorld)
    //         {
    //             if (vox.location == new Vector3Int(voxel.location.x, voxel.location.y - 1, 0))
    //             {
    //                 voxToCheckBot = vox;
    //             }
    //         }
    //
    //         if (voxToCheckBot != null)
    //         {
    //             var nNumber = n * voxToCheckBot.blockType.tileID;
    //             total += nNumber;
    //         }
    //
    //         // if (total != 0)
    //         // {
    //         //     Debug.Log(total);
    //         // }
    //         
    //         if(total != 0 && !voxel.isWater)
    //         {Debug.Log(total);}
    //         
    //
    //         if(total != 0 && !voxel.isWater)
    //         {
    //             BlockType nBlock = blockTypes[total];
    //             voxelsInWorld[i] = new Voxel(voxel.location, nBlock, nBlock.isWater);
    //         }
    //
    //         var spaw = Instantiate(txtComp, new Vector3(voxelsInWorld[i].location.x + .5f, voxelsInWorld[i].location.y + .5f),
    //             Quaternion.identity);
    //
    //         spaw.GetComponent<TextMeshPro>().text = total.ToString();
    //         
    //         tileMap.SetTile(new Vector3Int(voxelsInWorld[i].location.x, voxelsInWorld[i].location.y, 0), voxelsInWorld[i].blockType.tile);
    //     }
    // }
#endregion

    private void CreateWaterWorldData()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                voxelsInWorld.Add(new Vector2(x,y), new Voxel( new Vector3Int(x,y,0), blockTypes[17], blockTypes[17].isWater));
            }
        }
        CreateTiles();
    }

    private void CreateTiles()
    {
        foreach (var go in textilst)
        {
            Destroy(go);
        }
        
        for (int y = 1; y < 10 - 1; y++)
        {
            for (int x = 1; x < 10 - 1; x++)
            {
                var bitTotal = 0;
                // if (voxelsInWorld.TryGetValue(new Vector2(x, y), out Voxel voxToCheck))
                // {
                //     if (!voxToCheck.isWater)
                //     {
                //         Voxel voxNorth = null;
                //
                //         //NorthTile
                //         if (voxelsInWorld.TryGetValue(new Vector2(x, y - 1), out voxNorth))
                //         {
                //             Debug.Log("north " + voxNorth.blockType.tileID);
                //         }
                //         else
                //         {
                //             voxNorth = null;
                //         }
                //
                //         //WestTile
                //
                //         Voxel voxWest = null;
                //         if (voxelsInWorld.TryGetValue(new Vector2(x - 1, y), out voxWest))
                //         {
                //             Debug.Log("west " + voxWest.blockType.tileID);
                //         }
                //         else
                //         {
                //             voxWest = null;
                //         }
                //
                //         //NorthTile
                //         Voxel voxEast = null;
                //         if (voxelsInWorld.TryGetValue(new Vector2(x + 1, y), out voxEast))
                //         {           
                //             Debug.Log("east " + voxEast.blockType.tileID);
                //         }
                //         else
                //         {
                //             voxEast = null;
                //         }
                //
                //         //NorthTile
                //         Voxel voxSouth = null;
                //         if (voxelsInWorld.TryGetValue(new Vector2(x, y + 1), out voxSouth))
                //         {
                //             Debug.Log("South " + voxSouth.blockType.tileID);
                //         }
                //         else
                //         {
                //             voxSouth = null;
                //         }
                //
                //         bitTotal =     1 * voxNorth.blockType.tileID +
                //                        2 * voxWest.blockType.tileID +
                //                        4 * voxEast.blockType.tileID +
                //                        8 * voxSouth.blockType.tileID;
                //
                //         
                //         foreach (var block in blockTypes)
                //         {
                //             if(block.bitTotalNum == bitTotal)
                //             {
                //                 voxelsInWorld[new Vector2(x, y)] = new Voxel(new Vector3Int(x, y, 0),
                //                     block,
                //                     block.isWater);
                //             }
                //         }
                //     }
                // }
                
                tileMap.SetTile(new Vector3Int(x,y,0), voxelsInWorld[new Vector2(x,y)].blockType.tile);
                var textblock = Instantiate(txtComp, new Vector3(x+.5f, y+.5f,0), Quaternion.identity);
                textilst.Add(textblock);
                textblock.GetComponent<TextMeshPro>().text =
                    bitTotal.ToString(); // voxelsInWorld[new Vector2(x,y)].blockType.tileID.ToString();
            }
        }        
    }

    private void SetRightTiles()
    {
        for (int y = 1; y < 9; y++)
        {
            for (int x = 1; x < 9; x++)
            {
                Voxel voxNorth = null;
                //NorthTile
                if (voxelsInWorld.TryGetValue(new Vector2(x, y - 1), out voxNorth))
                {
                    Debug.Log("north " + voxNorth.blockType.tileID);
                }
                else
                {
                    voxNorth = null;
                }

                //WestTile

                Voxel voxWest = null;
                if (voxelsInWorld.TryGetValue(new Vector2(x - 1, y), out voxWest))
                {
                    Debug.Log("west " + voxWest.blockType.tileID);
                }
                else
                {
                    voxWest = null;
                }

                //NorthTile
                Voxel voxEast = null;
                if (voxelsInWorld.TryGetValue(new Vector2(x + 1, y), out voxEast))
                {
                    Debug.Log("east " + voxEast.blockType.tileID);
                }
                else
                {
                    voxEast = null;
                }

                //NorthTile
                Voxel voxSouth = null;
                if (voxelsInWorld.TryGetValue(new Vector2(x, y + 1), out voxSouth))
                {
                    Debug.Log("South " + voxSouth.blockType.tileID);
                }
                else
                {
                    voxSouth = null;
                }

                var bitTotal = 
                            1 * voxNorth.blockType.tileID +
                            2 * voxWest.blockType.tileID +
                            4 * voxEast.blockType.tileID +
                            8 * voxSouth.blockType.tileID;


                foreach (var block in blockTypes)
                {
                    if (block.bitTotalNum == bitTotal)
                    {
                        voxelsInWorld[new Vector2(x, y)] = new Voxel(new Vector3Int(x, y, 0),
                            block,
                            block.isWater);
                    }
                }
            }
        }
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3Int clickedTilePos = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            for (int i = 0; i < voxelsInWorld.Count; i++)
            {
                if (voxelsInWorld[new Vector2(clickedTilePos.x, clickedTilePos.y)].location == clickedTilePos)
                {
                    voxelsInWorld[new Vector2(clickedTilePos.x, clickedTilePos.y)] =
                        new Voxel(new Vector3Int(clickedTilePos.x, clickedTilePos.y, 0), blockTypes[18], false);
                    break;
                }
            }

            SetRightTiles();
            
            //Check north
            
            // if (voxelsInWorld.TryGetValue(new Vector2(clickedTilePos.x, clickedTilePos.y - 1), out Voxel voxNorth))
            // {
            //     int num = voxNorth.blockType.tileID * 8;
            //     
            //     voxelsInWorld[new Vector2(clickedTilePos.x, clickedTilePos.y)] =
            //         new Voxel(new Vector3Int(clickedTilePos.x, clickedTilePos.y, 0), blockTypes[num], blockTypes[num].isWater);
            //     if(Input.GetKey(KeyCode.A))
            //     {
            //         
            //         voxelsInWorld[new Vector2(clickedTilePos.x, clickedTilePos.y - 1)] =
            //             new Voxel(new Vector3Int(clickedTilePos.x, clickedTilePos.y, 0), blockTypes[1],
            //                 blockTypes[1].isWater);
            //     }
            // }
            
            CreateTiles();
        }
    }
    
}

[Serializable]
public class BlockType
{
    public Tile tile;
    public int tileID;
    public bool isWater;
    public int bitTotalNum;
}

[Serializable]
public class Voxel
{
    public Vector3Int location;
    public BlockType blockType;
    public bool isWater;
    
    public Voxel(Vector3Int location, BlockType blockType, bool isWater)
    {
        this.location = location;
        this.blockType = blockType;
        this.isWater = isWater;
    }
}
