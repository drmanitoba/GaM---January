using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
  
  [SerializeField]
  private Tile mainTilePrefab;

  private Bounds bounds;

  public Bounds RoomBounds {
    get { return bounds; }
  }

  void Awake() {
    bounds = new Bounds (Vector3.zero, Vector3.zero);
    BuildRoom ();
  }

  void OnDrawGizmos() {
    Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);
  }

  public void BuildRoom() {
    
    for (int i = 0; i <= 9; i++) {
      
      // Get world position of bottom left corner
      Vector3 bottomLeft = Vector3.zero;
      
      // Normalize position of block
      Vector3 blockOrigin = new Vector3 (bottomLeft.x + (mainTilePrefab.Width / 2), bottomLeft.y + (mainTilePrefab.Height / 2), 0);
      blockOrigin.x += i * (mainTilePrefab.Width);
      
      Tile tile = (Tile)Instantiate (mainTilePrefab, blockOrigin, Quaternion.identity);
      
      tile.transform.parent = transform;
    }
    
    for (int i = 0; i <= 9; i++) {

      // Get world position of top left corner
      Vector3 topLeft = new Vector3 (0, 10, 0);
      
      // Normalize position of block
      Vector3 blockOrigin = new Vector3 (topLeft.x + (mainTilePrefab.Width / 2), topLeft.y + -(mainTilePrefab.Height / 2), 0);
      blockOrigin.x += i * (mainTilePrefab.Width);
      
      Tile tile = (Tile)Instantiate (mainTilePrefab, blockOrigin, Quaternion.identity);
      
      tile.transform.parent = transform;
    }

    // Set room bounds to enacpsulate whole room
    Renderer[] renderers = GetComponentsInChildren<Renderer> ();
    foreach (Renderer renderer in renderers) {
      bounds.Encapsulate (renderer.bounds);
    }
  }
}