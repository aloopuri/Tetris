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

    public void Initialize() {
        this.cells = Data.Cells[this.tetromino];
    }
}