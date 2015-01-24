using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour {

  private TileManager tileManager;
  private Bounds bounds;
  private string[] roomRows;
  private Tile[][] roomTiles;
  private const int ROOM_WIDTH = 10;
  private const int ROOM_HEIGHT = 10;

  public Bounds RoomBounds {
    get { return bounds; }
  }

  void Awake() {
    bounds = new Bounds(Vector3.zero, Vector3.zero);
    tileManager = FindObjectOfType<TileManager>();
    roomTiles = new Tile[ROOM_HEIGHT][];
  }

  void OnDrawGizmos() {
    Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);
  }

  public void BuildRoom(string[] rows) {
    Tile[][] tilePrefabs = tilesForRoom(rows);

    drawRoom(tilePrefabs);

    // Set room bounds to enacpsulate whole room
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers) {
      bounds.Encapsulate(renderer.bounds);
    }
  }

  public Tile TopTileForColumn(int col) {
    Tile topTile;
    List<Tile> column = new List<Tile>();

    foreach (Tile[] row in roomTiles) {
      column.Add(row[col]);
    }

    topTile = column.FindLast(tile => tile != null && (tile.type == TileType.DirtPlatformMid || tile.type == TileType.DirtColumnTop));

    return topTile;
  }

  private void drawRoom(Tile[][] tiles) {
    
    // Get world position of bottom left corner
    Vector3 bottomLeft = Vector3.zero;

    int xOff;
    int yOff;

    for (yOff = 0; yOff < ROOM_HEIGHT; yOff++) {
      Tile[] row = tiles[yOff];
      roomTiles[yOff] = new Tile[ROOM_WIDTH];

      for (xOff = 0; xOff < ROOM_WIDTH; xOff++) {
        Tile tile = row[xOff];

        if (tile.type == TileType.Empty) {
          continue;
        }

        Tile currentTile;

        Vector3 blockOrigin = new Vector3(bottomLeft.x + (tile.Width / 2), bottomLeft.y + (tile.Height / 2), 0);
        blockOrigin.x += xOff * tile.Width;
        blockOrigin.y += yOff * tile.Height;
        
        currentTile = (Tile)Instantiate(tile, blockOrigin, Quaternion.identity);
        
        currentTile.transform.parent = transform;

        roomTiles[yOff][xOff] = currentTile;
      }
    }
  }

  private Tile[][] tilesForRoom(string[] rows) {
    int yOff;

    Tile[][] tiles = new Tile[ROOM_HEIGHT][];

    roomRows = rows;

    for (yOff = 0; yOff < ROOM_HEIGHT; yOff++) {
      tiles[yOff] = tilesForRow(rows[yOff], yOff);
    }

    return tiles;
  }

  private Tile[] tilesForRow(string row, int rowIdx) {
    int xOff;

    Tile[] rowTiles = new Tile[ROOM_WIDTH];

    for (xOff = 0; xOff < ROOM_WIDTH; xOff++) {
      TileType tileType = getTileType(xOff, rowIdx);
      Tile tilePrefab;
          
      tilePrefab = tileManager.GetTile(tileType);
          
      rowTiles[xOff] = tilePrefab;
    }
    
    return rowTiles;
  }

  private TileType getTileType(int x, int y) {
    char blockCode = roomRows[y][x];
    TileType type = TileType.Empty;

    if (blockCode == '1') {

      if (y <= (ROOM_HEIGHT / 2)) {
        type = TileType.DirtBlock;
      } else {
        type = TileType.DownwardDirtBlock;
      }

      // Platform or Column
      if (emptyAbove(x, y)) {

        // Middle
        if ((blockLeft(x, y) || edgeLeft(x, y)) &&
            (blockRight(x, y) || edgeRight(x, y))) {
            type = TileType.DirtPlatformMid;
        }

        // Left
        if (emptyLeft(x, y) && blockRight(x, y)) {
          type = TileType.DirtPlatformLeft;
        }

        // Right
        if (emptyRight(x, y) && blockLeft(x, y)) {
          type = TileType.DirtPlatformRight;
        }

        // Column Top
        if ((emptyRight(x, y) || edgeRight(x, y)) && (emptyLeft(x, y) || edgeLeft(x, y))) {
          type = TileType.DirtColumnTop;
        }
      }

      // Top Edge
      else if (y == ROOM_HEIGHT - 1 && emptyBelow(x, y)) {
        type = TileType.DirtTopEdge;
      }

      // Downwards Column Bottom Cap
      else if (blockAbove(x, y) && (emptyBelow(x, y) && !edgeBelow(x, y))) {
        type = TileType.DirtColumnBottom;
      }
    }

    return type;
  }

  private bool blockAbove(int xOff, int yOff) {
    int yOffset = yOff < 9 ? yOff + 1 : 9;

    char aboveCode = roomRows[yOffset][xOff];

    return aboveCode == '1';
  }

  private bool edgeAbove(int xOff, int yOff) {
    return yOff == 9;
  }

  private bool blockBelow(int xOff, int yOff) {
    int yOffset = yOff > 0 ? yOff - 1 : 0;

    char belowCode = roomRows[yOffset][xOff];
    
    return belowCode == '1';
  }

  private bool edgeBelow(int xOff, int yOff) {
    return yOff == 0;
  }

  private bool blockLeft(int xOff, int yOff) {
    if (xOff == 0) { return false; }

    int xOffset = xOff - 1;
    
    char leftCode = roomRows[yOff][xOffset];
    
    return leftCode == '1';
  }

  private bool edgeLeft(int xOff, int yOff) {
    if (xOff == 0) {
      return true;
    }

    return false;
  }

  private bool blockRight(int xOff, int yOff) {
    if (xOff == ROOM_WIDTH - 1) { return false; }
    
    int xOffset = xOff + 1;
    
    char rightCode = roomRows[yOff][xOffset];
    
    return rightCode == '1';
  }

  private bool edgeRight(int xOff, int yOff) {
    if (xOff == ROOM_WIDTH - 1) {
      return true;
    }
    
    return false;
  }

  private bool emptyAbove(int xOff, int yOff) {
    int yOffset = yOff < ROOM_HEIGHT - 1 ? yOff + 1 : ROOM_HEIGHT - 1;
    
    char aboveCode = roomRows[yOffset][xOff];
    
    return aboveCode == '0';
  }

  private bool emptyBelow(int xOff, int yOff) {
    int yOffset = yOff > 0 && yOff < ROOM_HEIGHT - 1 ? yOff - 1 : ROOM_HEIGHT - 2;
    
    char aboveCode = roomRows[yOffset][xOff];
    
    return aboveCode == '0';
  }
  
  private bool emptyLeft(int xOff, int yOff) {
    if (xOff == 0) { return false; }
    
    int xOffset = xOff - 1;
    
    char leftCode = roomRows[yOff][xOffset];
    
    return leftCode == '0';
  }

  private bool emptyRight(int xOff, int yOff) {
    if (xOff == ROOM_WIDTH - 1) { return false; }
    
    int xOffset = xOff + 1;
    
    char rightCode = roomRows[yOff][xOffset];
    
    return rightCode == '0';
  }
}