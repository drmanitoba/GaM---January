using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

  // movement config
  public float gravity = -25f;
  public float runSpeed = 8f;
  public float groundDamping = 20f; // how fast do we change direction? higher means faster
  public float inAirDamping = 5f;
  public float jumpHeight = 3f;
  
  [HideInInspector]
  private float normalizedHorizontalSpeed = 0;

  [SerializeField]
  private CharacterController2D playerPrefab;
  private CharacterController2D _controller;
  private Animator _animator;
  private RaycastHit2D _lastControllerColliderHit;
  private Vector3 _velocity;
  
  private int walkAnimHash;
  private int stillAnimHash;
  
  
  void Awake()
  {
    _controller = (CharacterController2D) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    _animator = _controller.GetComponent<Animator>();

    walkAnimHash = Animator.StringToHash("PlayerWalk");
    stillAnimHash = Animator.StringToHash("PlayerStill");
    
    // listen to some events for illustration purposes
    _controller.onControllerCollidedEvent += onControllerCollider;
    _controller.onTriggerEnterEvent += onTriggerEnterEvent;
    _controller.onTriggerExitEvent += onTriggerExitEvent;
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
    _velocity = _controller.velocity;
    
    if( _controller.isGrounded )
      _velocity.y = 0;
     
    if( Input.GetKey( KeyCode.RightArrow ) )
    {
      normalizedHorizontalSpeed = _controller.isGrounded ? 1 : 0.3f;

      if (_animator.GetBool("Walking") == false) {
        _animator.SetBool("Walking", true);
      }

      if( _controller.transform.localScale.x < 0f )
        _controller.transform.localScale = new Vector3( -_controller.transform.localScale.x, _controller.transform.localScale.y, _controller.transform.localScale.z );
    }
    else if( Input.GetKey( KeyCode.LeftArrow ) )
    {
      normalizedHorizontalSpeed = _controller.isGrounded ? -1 : -0.3f;

      if (_animator.GetBool("Walking") == false) {
        _animator.SetBool("Walking", true);
      }

      if( _controller.transform.localScale.x > 0f )
        _controller.transform.localScale = new Vector3( -_controller.transform.localScale.x, _controller.transform.localScale.y, _controller.transform.localScale.z );
    }
    else
    {
      if (_animator.GetBool("Walking") == true) {
        _animator.SetBool("Walking", false);
      }

      normalizedHorizontalSpeed = 0;
    }
    
    
    // we can only jump whilst grounded
    if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
    {
      _velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
//      _animator.Play( Animator.StringToHash( "Jump" ) );
    }
    
    
    // apply horizontal speed smoothing it
    var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
    _velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
    
    // apply gravity before moving
    _velocity.y += gravity * Time.deltaTime;
    
    _controller.move( _velocity * Time.deltaTime );
  }
}
