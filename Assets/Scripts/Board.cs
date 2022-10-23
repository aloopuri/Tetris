using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;

    private void Awake() {
        // tilemap is a child of the game object that board script is attached to
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i=0; i<tetrominoes.Length; i++) {
            this.tetrominoes[i].Initialize();            
        }
    }

    private void Start() {
        SpawnPiece();
    }

    // Chooses a random tetromino piece
    public void SpawnPiece() {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, spawnPosition, data);
        Set(this.activePiece);
    }

    // Sets piece on the board
    public void Set(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    
}
