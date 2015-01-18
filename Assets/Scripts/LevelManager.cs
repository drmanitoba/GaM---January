using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour {

  [SerializeField]
  private Room roomPrefab;

  private string fileName = "LevelsTest.txt";

  private List<string[]> roomSeeds;

  private List<Room> rooms;

  private Bounds levelBounds;

  public Bounds LevelBounds {
    get { return levelBounds; }
  }

  // Use this for initialization
  void Start() {

    levelBounds = new Bounds(Vector3.zero, Vector3.zero);

    roomSeeds = new List<string[]>();
    rooms = new List<Room>();

    readRooms();
    initLevel();
  }

  void OnDrawGizmos() {
    Gizmos.DrawWireCube(transform.position + levelBounds.center, levelBounds.size);
  }
  
  // Update is called once per frame
  void Update() {
  }

  private void readRooms() {
    string roomSeed = File.ReadAllText(Application.dataPath + "/Levels/" + fileName);
    string[] separators = {"\n\n", "\r\r", "\r\n\r\n"};
    string[] rooms = roomSeed.Split(separators,
                                    StringSplitOptions.RemoveEmptyEntries);

    foreach (string room in rooms) {
      string[] roomRows;

      roomRows = room.Split("\n"[0])
                     .Where(x => !string.IsNullOrEmpty(x))
                     .ToArray();

      
      Array.Reverse(roomRows);

      roomSeeds.Add(roomRows);
    }
  }

  private void initLevel() {
    int startingRooms = 5;

    for (int i = 0; i <= startingRooms; i++) {
      float rand = UnityEngine.Random.value;
      
      int randIdx = (int)(rand * roomSeeds.Count);

      int previousIdx = i > 0 ? i - 1 : 0;

      Vector3 roomPosition;
      Room previousRoom;

      Room room = (Room)Instantiate(roomPrefab);

      if (i == 0) {
        previousRoom = room;
        roomPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1));
      } else {
        previousRoom = rooms[previousIdx];
        roomPosition = previousRoom.transform.position + new Vector3(previousRoom.RoomBounds.size.x, 0, 0);
      }

      rooms.Add(room);

      room.BuildRoom(roomSeeds[randIdx]);

      room.transform.position = roomPosition;
      room.transform.parent = transform;
    }

    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers) {
      levelBounds.Encapsulate(renderer.bounds);
    }
  }
}
