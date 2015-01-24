using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

  // movement config
  public float gravity = -25f;
  public float runSpeed = 4f;
  public float groundDamping = 20f; // how fast do we change direction? higher means faster
  public float inAirDamping = 5f;
  public float jumpHeight = 3f;
  
  [HideInInspector]
  private float normalizedHorizontalSpeed = 0;

  [SerializeField]
  private CharacterController2D playerPrefab;
  private CharacterController2D player;
  private Animator animator;
  private LevelManager levelManager;
  private RaycastHit2D lastControllerColliderHit;
  private Vector3 velocity;

  void Awake()
  {
    LevelManager.OnLevelInit += HandleOnLevelInit;
  }

  void HandleOnLevelInit (LevelManager manager)
  {
    levelManager = manager;
    player = spawnPlayer();
    animator = player.GetComponent<Animator>();
    
    // listen to some events for illustration purposes
    player.onControllerCollidedEvent += onControllerCollider;
    player.onTriggerEnterEvent += onTriggerEnterEvent;
    player.onTriggerExitEvent += onTriggerExitEvent;
  }
  
  
  #region Event Listeners
  
  void onControllerCollider( RaycastHit2D hit )
  {
    // bail out on plain old ground hits cause they arent very interesting
    if( hit.normal.y == 1f )
      return;

    // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
    //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
  }
  
  
  void onTriggerEnterEvent( Collider2D col )
  {
    Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
  }
  
  
  void onTriggerExitEvent( Collider2D col )
  {
    Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
  }
  
  #endregion
  
  
  // the Update loop contains a very simple example of moving the character around and controlling the animation
  void Update()
  {
    // grab our current _velocity to use as a base for all calculations
    velocity = player.velocity;
    
    if( player.isGrounded ) {
      velocity.y = 0;

      if (animator.GetBool("Jumping") == true) {
        animator.SetBool("Jumping", false);
      }
    }
     
    if( Input.GetAxis("Horizontal") > 0 )
    {
      normalizedHorizontalSpeed = 1;

      if (player.isGrounded && animator.GetBool("Walking") == false) {
        animator.SetBool("Walking", true);
      }

      if( player.transform.localScale.x < 0f )
        player.transform.localScale = new Vector3( -player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z );
    }
    else if( Input.GetAxis("Horizontal") < 0 )
    {
      normalizedHorizontalSpeed = -1;

      if (player.isGrounded && animator.GetBool("Walking") == false) {
        animator.SetBool("Walking", true);
      }

      if( player.transform.localScale.x > 0f )
        player.transform.localScale = new Vector3( -player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z );
    }
    else
    {
      if (animator.GetBool("Walking") == true) {
        animator.SetBool("Walking", false);
      }

      normalizedHorizontalSpeed = 0;
    }
    
    
    // we can only jump whilst grounded
    if( player.isGrounded && Input.GetButtonDown("Jump") )
    {

      if (animator.GetBool("Walking") == true) {
        animator.SetBool("Walking", false);
      }

      animator.SetBool("Jumping", true);

      velocity.y = Mathf.Sqrt( (runSpeed / 2) * jumpHeight * -gravity );
//      _animator.Play( Animator.StringToHash( "Jump" ) );
    }

    if (Input.GetButtonDown("Run")) {
      runSpeed += 4;
    } else if (Input.GetButtonUp("Run")) {
      runSpeed -= 4;
    }
    
    // apply horizontal speed smoothing it
    var smoothedMovementFactor = player.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
    velocity.x = Mathf.Lerp( velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
    
    // apply gravity before moving
    velocity.y += gravity * Time.deltaTime;
    
    player.move( velocity * Time.deltaTime );
  }

  private CharacterController2D spawnPlayer() {
    Vector3 spawnPoint;
    Vector3 firstBlock = getFirstBlockCoords();

    spawnPoint = firstBlock + new Vector3(0, playerPrefab.Height, 0);

    return (CharacterController2D) Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
  }

  private Vector3 getFirstBlockCoords() {
    Tile spawnTile = levelManager.GetSpawnTile();
    Vector3 firstBlockCoords = spawnTile == null ? Vector3.zero : spawnTile.transform.position;

    return firstBlockCoords;
  }
}
