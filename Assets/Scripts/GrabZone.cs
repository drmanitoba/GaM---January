using UnityEngine;
using System;
using System.Collections.Generic;

public class GrabZone : MonoBehaviour {

  public event Action<Collider2D> onTriggerEnterEvent;
  public event Action<Collider2D> onTriggerStayEvent;
  public event Action<Collider2D> onTriggerExitEvent;

  public void OnTriggerEnter2D( Collider2D col )
  {
    if( onTriggerEnterEvent != null )
      onTriggerEnterEvent( col );
  }
  
  
  public void OnTriggerStay2D( Collider2D col )
  {
    if( onTriggerStayEvent != null )
      onTriggerStayEvent( col );
  }
  
  
  public void OnTriggerExit2D( Collider2D col )
  {
    if( onTriggerExitEvent != null )
      onTriggerExitEvent( col );
  }
}
