using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board {get; private set; }
    public Vector3Int position {get; private set; }
    public TetrominoData data {get; private set; }
    public Vector3Int[] cells {get; private set; }
    public int rotationIndex {get; private set; }

    // Controls how fast a piece drops
    public float stepDelay = 1f;
    // Controls how long a piece takes before it locks into place
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    // Controls delay for pieces to continuously move in a direction
    private float horizontalHoldDelay = 0.05f;
    private float verticalHoldDelay = 0.08f;
    private float keyDownWaitDelay = 0.12f;

    private float horizontalHoldTime;
    private float verticalHoldTime;
    private float keyDownWaitTimer;

    private bool keyHeldDown;

    public void Initialize(Board board, Vector3Int position, TetrominoData data) {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;
        this.horizontalHoldTime = 0f;
        this.verticalHoldTime = 0f;
        this.keyHeldDown = false;

        if (this.cells == null) {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
        
    }

    // Handles input. Run every frame.
    private void Update() {
        this.board.Clear(this);

        this.lockTime += Time.deltaTime;

        // TO DO: ADD DELAY WHEN HELD DOWN OF INDIVIDUAL PRESSES STILL WORK PROPERLY
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S)) {
            horizontalHoldTime = 0f;
            verticalHoldTime = 0f;
            keyDownWaitTimer = 0f;
            keyHeldDown = false;
        }

        // Rotation
        if (Input.GetKeyDown(KeyCode.Q)) {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            Rotate(1);
        }

        // Horizontal movemeent
        if (Input.GetKey(KeyCode.A)) {

            if (keyHeldDown) {
                if (keyDownWaitTimer < keyDownWaitDelay) {
                    keyDownWaitTimer += Time.deltaTime;
                    return;
                }

                if (horizontalHoldTime < horizontalHoldDelay) {
                    horizontalHoldTime += Time.deltaTime;
                    return;
                }
                horizontalHoldTime = 0;
            }
            
            if (!keyHeldDown) {
                keyHeldDown = true;
            }

            Move(Vector2Int.left);
        }
        else if (Input.GetKey(KeyCode.D)) {
            if (keyHeldDown) {
                if (keyDownWaitTimer < keyDownWaitDelay) {
                    keyDownWaitTimer += Time.deltaTime;
                    return;
                }

                if (horizontalHoldTime < horizontalHoldDelay) {
                    horizontalHoldTime += Time.deltaTime;
                    return;
                }
                horizontalHoldTime = 0;
            }

            if (!keyHeldDown) {
                keyHeldDown = true;
            }
            Move(Vector2Int.right);
        }

        // moving down/dropping
        if (Input.GetKey(KeyCode.S)) {
            if (keyHeldDown) {
                if (verticalHoldTime < verticalHoldDelay) {
                    verticalHoldTime += Time.deltaTime;
                    return;
                }
                verticalHoldTime = 0;
            }

            if (!keyHeldDown) {
                keyHeldDown = true;
            }

            if (Move(Vector2Int.down)) {
                this.board.incrementScore();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            HardDrop();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            this.board.SwapPiece(this.data);
        }

        if (Time.time >= this.stepTime) {
            Step();
        }

        this.board.Set(this);
    }

    private void Step() {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if (this.lockTime >= lockDelay) {
            Lock();
        }
    }

    private void HardDrop() {
        int rowsdropped = 0;
        while (Move(Vector2Int.down)) {
            rowsdropped++;
            continue;
        }

        Lock();
        if (rowsdropped > 0) {
            this.board.AddHardDropScore(rowsdropped);
        }
    }

    // TO DO LATER: when game over, check before spawning new piece, after ClearLines()
    private void Lock() {
        this.board.Set(this);
        this.board.ClearLines();
        // here
        this.board.SpawnPiece();
    }

    private bool Move(Vector2Int translation) {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid) {
            this.position = newPosition;
            this.lockTime = 0f;
        }
        return valid;
    }

    private void Rotate(int direction) {
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        int originalRotation = this.rotationIndex;
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction)) {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);     
        }
    }

    private void ApplyRotationMatrix(int direction) {
         // Loop over cells.Length instead of data.cells as this piece rotates
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3 cell = this.cells[i];
            int x, y;

            switch (this.data.tetromino) {
                case Tetromino.I:
                case Tetromino.O:
                    // I and O pieces are rotated from an offset centre point
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }
            
            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection) {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for(int i = 0; i< this.data.wallKicks.GetLength(1); i++) {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];
            if (Move(translation)) {
                return true;
            }
        }
        
        return false;
    }

    // This method returns the index position of the rotation type in Data
    // based on the list in wall kick data table https://tetris.fandom.com/wiki/SRS
    private int GetWallKickIndex(int rotationIndex, int rotationDirection) {
        int wallKickIndex = rotationIndex * 2;
        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max) {
        if (input < min) {
            return max - (min - input) % (max - min);
        }
        else {
            return min + (input - min) % (max - min);
        }
    }
}
