using UnityEngine;
using System.Collections;
using System;

public class Room : MonoBehaviour {
  
  [SerializeField]
  private Tile mainTilePrefab;
  private Bounds bounds;

  public Bounds RoomBounds {
    get { return bounds; }
  }

  void Awake() {
    bounds = new Bounds(Vector3.zero, Vector3.zero);
  }

  void OnDrawGizmos() {
    Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);
  }

  public void BuildRoom(string[] rows) {

    int xOff;
    int yOff;

    // Get world position of bottom left corner
    Vector3 bottomLeft = Vector3.zero;

    Array.Reverse(rows);
    
    for (yOff = 0; yOff <= 9; yOff++) {
      for (xOff = 0; xOff <= 9; xOff++) {

        char blockCode = rows[yOff][xOff];

        if (blockCode == '0') {
          continue;
        }

        if (blockCode == '1') {
          // Normalize position of block
          Vector3 blockOrigin = new Vector3(bottomLeft.x + (mainTilePrefab.Width / 2), bottomLeft.y + (mainTilePrefab.Height / 2), 0);
          blockOrigin.x += xOff * mainTilePrefab.Width;
          blockOrigin.y += yOff * mainTilePrefab.Height;
      
          Tile tile = (Tile)Instantiate(mainTilePrefab, blockOrigin, Quaternion.identity);
      
          tile.transform.parent = transform;
        }
      }
    }

    // Set room bounds to enacpsulate whole room
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers) {
      bounds.Encapsulate(renderer.bounds);
    }
  }
}