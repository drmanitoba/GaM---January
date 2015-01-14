using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(SpriteRenderer))]
public class DisplayObject : MonoBehaviour {
  protected SpriteRenderer _spriteRenderer;

  public SpriteRenderer spriteRenderer {
    get {
      return _spriteRenderer;
    }
  }

  public Sprite sprite {
    get {
      return _spriteRenderer != null ? _spriteRenderer.sprite : null;
    }
    set {
      if (_spriteRenderer != null) {
        _spriteRenderer.sprite = value;
      }
    }
  }

  private float _screenResolutionScale = (float)Screen.height / (float)Screen.width;

  public float Width {
    get {
      if (_spriteRenderer && _spriteRenderer.sprite != null) {
        return _spriteRenderer.sprite.textureRect.width * _screenResolutionScale;
      } else {
        return renderer.bounds.size.x;
      }
    }
  }

  public float Height {
    get {
      if (_spriteRenderer && _spriteRenderer.sprite != null) {
        return _spriteRenderer.sprite.textureRect.height * _screenResolutionScale;
      } else {
        return renderer.bounds.size.y;
      }
    }
  }

  public void Awake() {
    Initialize ();
  }

  protected virtual void Initialize() {
    if (GetComponent<SpriteRenderer> () != null) {
      _spriteRenderer = GetComponent<SpriteRenderer> ();
    } else {
      _spriteRenderer = gameObject.AddComponent<SpriteRenderer> ();
    }
  }
}