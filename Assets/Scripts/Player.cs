using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	enum PlayerState {
		GROUNDED,
		JUMPING,
		FALLING
	}

	private PlayerState currentState;
	private Animator animator;

	public void Awake() {
		animator = GetComponent<Animator>();
	}

	public void Update() {
		if (isJumping()) {
			animator.SetBool("Jumping", true);
		} else if (isFalling()) {
			animator.SetBool("Jumping", false);
		}
	}

	public void StartJump() {
		currentState = PlayerState.JUMPING;
	}

	public void StopJump() {
		currentState = PlayerState.FALLING;
	}

	private bool isJumping() {
		return currentState == PlayerState.JUMPING;
	}

	private bool isFalling() {
		return currentState == PlayerState.FALLING;
	}
}