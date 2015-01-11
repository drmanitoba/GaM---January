using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private Player player;

	public Player Player {
		get { return player; }
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Jump")) {
			player.StartJump();
		} else if (Input.GetButtonUp("Jump")) {
			player.StopJump();
		}
	}
}
