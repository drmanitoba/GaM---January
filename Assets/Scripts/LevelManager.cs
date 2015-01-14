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

  // Use this for initialization
  void Start() {

    roomSeeds = new List<string[]>();

    ReadRooms();

    Room room1 = (Room)Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
    Room room2 = (Room)Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
//    Room room3 = (Room)Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

    room1.BuildRoom(roomSeeds[0]);
    room2.BuildRoom(roomSeeds[1]);

    room1.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1));
    room2.transform.position = room1.transform.position;
    room2.transform.position += new Vector3(room1.RoomBounds.size.x, 0, 0);
//    room3.transform.position = room2.transform.position;
//    room3.transform.position += new Vector3(room1.RoomBounds.size.x, 0, 0);
  }
  
  // Update is called once per frame
  void Update() {
  }

  private void ReadRooms() {
    string roomSeed = File.ReadAllText(Application.dataPath + "/Levels/" + fileName);
    string[] separators = {"\n\n", "\r\r", "\r\n\r\n"};
    string[] rooms = roomSeed.Split(separators,
                                    StringSplitOptions.RemoveEmptyEntries);

    foreach (string room in rooms) {
      string[] roomRows;

      roomRows = room.Split("\n"[0])
                     .Where(x => !string.IsNullOrEmpty(x))
                     .ToArray();

      roomSeeds.Add(roomRows);
    }
  }
}
