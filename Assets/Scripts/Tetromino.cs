using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino {
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
}

[System.Serializable]
public struct TetrominoData {
    public Tetromino tetromino;
    public Tile tile;
    // Allows manual assignment of coordinates for custom shapes in editor 
    // public Vector2Int[] cells;   // This shows in editor (in 'board' tilemap inspector), C# field 
    public Vector2Int[] cells { get; private set; } // C# property
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize() {
        // gets the data for the shape of each tetromino
        this.cells = Data.Cells[this.tetromino];
        this.wallKicks = Data.WallKicks[this.tetromino];
    }
}