using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LevelManager : MonoBehaviour {

  [SerializeField]
  private Room roomPrefab;

  private string fileName = "LevelsTest.txt";

  // Use this for initialization
  void Start() {

    string[] roomRows = File.ReadAllLines(Application.dataPath + "/Levels/" + fileName);
    Array.Reverse(roomRows);

    Room room1 = (Room)Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
//    Room room2 = (Room)Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
//    Room room3 = (Room)Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

    room1.BuildRoom(roomRows);

    room1.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1));
//    room2.transform.position = room1.transform.position;
//    room2.transform.position += new Vector3(room1.RoomBounds.size.x, 0, 0);
//    room3.transform.position = room2.transform.position;
//    room3.transform.position += new Vector3(room1.RoomBounds.size.x, 0, 0);
  }
  
  // Update is called once per frame
  void Update() {
  }
}
