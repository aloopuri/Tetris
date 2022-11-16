using UnityEngine;
using UnityEngine.Tilemaps;

public class NextPiece : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Board board;
    // public TetrominoData[] tetrominoes;
    public Vector3Int position;

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
    }



    // public void Initialize(TetrominoData[] tetrominoes) {
    //     this.tetrominoes = tetrominoes;
    // }

    // public 

}
