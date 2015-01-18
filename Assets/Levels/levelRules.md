* Read in row-wise from bottom to top
* Empty '0'
  * Skip and move on
* Tile 'non 0'
  * Solid tile '1'
    * If there's a block above it
      * If it's also a '1'
        * Draw dirt tile
    * If there's a block on both sides
      * Draw a middle top tile
    * If there's a block only on the right
      * Draw a left edge tile
