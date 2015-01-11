using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

  [SerializeField]
  private Room roomPrefab;

  // Use this for initialization
  void Start() {
    Room room = (Room)Instantiate (roomPrefab, Vector3.zero, Quaternion.identity);
    Debug.Log (Camera.main.ScreenToWorldPoint(room.RoomBounds.size));
  }
  
  // Update is called once per frame
  void Update() {
  }
}
