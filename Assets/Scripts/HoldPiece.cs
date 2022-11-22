using UnityEngine;
using UnityEngine.Tilemaps;

public class HoldPiece : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tile grayTile;
    public Board board;
    public TetrominoData holdPiece {get; private set; }
    public Vector3Int position { get; private set; }
    public bool hasSwapped = false;

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.position = tilemap.WorldToCell(transform.position);
    }

    public void StorePiece(TetrominoData data) {
        if (this.holdPiece.cells != null) {
            Clear();
        }
        this.holdPiece = data;
        Set(data);
    }

    public TetrominoData GetHoldPiece(TetrominoData data) {
        TetrominoData temp = this.holdPiece;
        StorePiece(data);
        this.hasSwapped = true;
        return temp;

    }

    public bool Exists() {
        if (this.holdPiece.cells == null) {
            return false;
        }
        return true;
    }

    private void Set(TetrominoData data) {
        for (int i = 0; i < data.cells.Length; i++) {
            Vector3Int tilePosition = (Vector3Int)data.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, data.tile);
        }
    }

    private void Clear() {
        for (int i = 0; i < holdPiece.cells.Length; i++) {
            Vector3Int tilePosition = (Vector3Int)holdPiece.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

}
