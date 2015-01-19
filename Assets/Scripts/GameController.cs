using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

  [SerializeField]
  private Player player;

  public Player Player {
    get { return player; }
  }

	// Use this for initialization
	void Awake () {
	  LevelManager.OnLevelInit += HandleOnLevelInit;
	}

  public void HandleOnLevelInit() {
    LevelManager.OnLevelInit -= HandleOnLevelInit;
    Instantiate(player);
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
