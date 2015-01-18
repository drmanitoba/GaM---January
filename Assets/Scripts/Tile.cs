using UnityEngine;
using System.Collections;

public class Tile : DisplayObject {

  protected override void Initialize() {
    base.Initialize();
  }
}

public enum TileType {
  Empty,
  DirtBlock,
  DirtColumnBottom,
  DirtColumnTop,
  DirtPlatformLeft,
  DirtPlatformMid,
  DirtPlatformRight,
  DirtTopEdge,
  DownwardDirtBlock
}