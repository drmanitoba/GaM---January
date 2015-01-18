using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

  [SerializeField]
  private Tile dirtBlock;

  [SerializeField]
  private Tile dirtColumnBottom;

  [SerializeField]
  private Tile dirtColumnTop;

  [SerializeField]
  private Tile dirtPlatformLeft;

  [SerializeField]
  private Tile dirtPlatformMid;

  [SerializeField]
  private Tile dirtPlatformRight;

  [SerializeField]
  private Tile dirtTopEdge;

  [SerializeField]
  private Tile downwardDirtBlock;

  private Dictionary<TileType, Tile> tileMap;

  public void Awake() {
    tileMap = new Dictionary<TileType, Tile>();

    tileMap.Add(TileType.Empty, null);
    tileMap.Add(TileType.DirtBlock, dirtBlock);
    tileMap.Add(TileType.DirtColumnBottom, dirtColumnBottom);
    tileMap.Add(TileType.DirtColumnTop, dirtColumnTop);
    tileMap.Add(TileType.DirtPlatformLeft, dirtPlatformLeft);
    tileMap.Add(TileType.DirtPlatformMid, dirtPlatformMid);
    tileMap.Add(TileType.DirtPlatformRight, dirtPlatformRight);
    tileMap.Add(TileType.DirtTopEdge, dirtTopEdge);
    tileMap.Add(TileType.DownwardDirtBlock, downwardDirtBlock);
  }

  public Tile GetTile(TileType type) {
    return tileMap[type];
  }
}
