using UnityEngine;
using UnityEngine.Tilemaps;

public class NextPiece : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Board board;
    public TetrominoData nextPiece { get; private set; }
    public Vector3Int position { get; private set;}

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.position = tilemap.WorldToCell(transform.position);
    }

    // private void LateUpdate(){
    //     GeneratePiece();
    // }

    public void GeneratePiece() {
        int random = Random.Range(0, this.board.tetrominoes.Length);
        TetrominoData data = this.board.tetrominoes[random];
        
        if (nextPiece.cells != null) {
            Clear();
        }
        this.nextPiece = data;
        Set(data);        
    }

    private void Set(TetrominoData data) {
        for (int i = 0; i < data.cells.Length; i++) {
            Vector3Int tilePosition = (Vector3Int)data.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, data.tile);
        }
    }

    private void Clear() {
        for (int i = 0; i < nextPiece.cells.Length; i++) {
            Vector3Int tilePosition = (Vector3Int)nextPiece.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    // public void Initialize(TetrominoData[] tetrominoes) {
    //     this.tetrominoes = tetrominoes;
    // }

    // public 

}
