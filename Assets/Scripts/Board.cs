using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Piece activePiece { get; private set; }
    public Hud hud;
    public NextPiece nextPiece;
    public HoldPiece holdPiece;
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10,20);
    
    

    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

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
        TetrominoData data;
        if (this.nextPiece.nextPiece.cells == null) {
            int random = Random.Range(0, this.tetrominoes.Length);
            data = this.tetrominoes[random];
            nextPiece.GeneratePiece();
        }
        else {
            data = nextPiece.GetNextPiece();       

        }

        this.holdPiece.hasSwapped = false;
        SpawnPiece(data);

        // this.activePiece.Initialize(this, spawnPosition, data);

        // if (IsValidPosition(this.activePiece, this.spawnPosition)) {
        //     Set(this.activePiece);        
        // }
        // else {
        //     GameOver();
        // }
    }

    private void SpawnPiece(TetrominoData data) {
        if (this.holdPiece.Exists() && !this.holdPiece.hasSwapped) {
            this.holdPiece.RecolourToOriginal();
        }
        this.activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(this.activePiece, this.spawnPosition)) {
            Set(this.activePiece);        
        }
        else {
            GameOver();
        }
    }

    public void SwapPiece(TetrominoData curPiece) {
        if (this.holdPiece.hasSwapped) {
            return;
        }
        TetrominoData data;
        if (!this.holdPiece.Exists()) {
            data = this.nextPiece.GetNextPiece();
            this.holdPiece.StorePiece(curPiece); 
            this.holdPiece.hasSwapped = true;
            SpawnPiece(data);
            return;
        }
        data = this.holdPiece.GetHoldPiece(curPiece);
        SpawnPiece(data);
        this.holdPiece.hasSwapped = true;

    }

    // Game resets due to line clear as the board is full
    // A new piece still spawns and because it cant move it locks and
    // the game checks for line clears, subsequently clearing the board.
    private void GameOver() {
        RectInt bounds = this.Bounds;

        for (int row = bounds.yMin; row < bounds.yMax; row++) {
            for (int col = bounds.xMin; col < bounds.xMax; col++) {
                Vector3Int position = new Vector3Int(col, row, 0);
                int rand = Random.Range(0, tetrominoes.Length);

                TileBase tile = tetrominoes[rand].tile;
                this.tilemap.SetTile(position, tile);
            }
        }
    }

    // Sets piece on the board
    public void Set(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    // Clears piece on the board
    public void Clear(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position) {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }
            if (this.tilemap.HasTile(tilePosition)) {
                return false;
            }
        }
        return true;
    }

    // clears full rows and updates score
    public void ClearLines() {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int linesCleared = 0;

        while (row < bounds.yMax) {
            if (IsLineFull(row)) {
                LineClear(row);
                linesCleared++;
            }
            else {
                row++;
            }
        }

        hud.AddLineClearedSCore(linesCleared);
    }

    public void AddHardDropScore(int rows) {
        hud.UpdateScore(rows + 1);
    }

    public void incrementScore() {
        hud.UpdateScore(1);
    }
    
    

    private bool IsLineFull(int row) {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++) {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position)) {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row) {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++) { 
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);

        }

        while (row < bounds.yMax) {
            for (int col = bounds.xMin; col < bounds.xMax; col++) { 
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }
    }
    
}
