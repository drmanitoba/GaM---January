using UnityEngine;
using System.Collections;

public class Tile : DisplayObject {

  public TileType type;
  public bool DrawCube = false;

  private int xOffset;
  private int yOffset;

  public int XOffset {
    get { return xOffset; }
    set { xOffset = value; }
  }

  public int YOffset {
    get { return yOffset; }
    set { yOffset = value; }
  }

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