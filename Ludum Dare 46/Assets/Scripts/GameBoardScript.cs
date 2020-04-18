using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardScript : MonoBehaviour {

    public bool shouldPlay = false;
    public bool playIndefinitely = false;
    public int playSteps = 0;
    public RawImage boardImage;

    private List<List<GameCellScript>> boardArray; // TODO: figure out type. Probably custom class

    private Texture2D boardTexture;

    void Awake() {
        // TODO: make boardArray
        boardArray = new List<List<GameCellScript>>();

        // Just default to a 64x64 array for now
        boardTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);

        if (boardImage == null) {
            boardImage = GetComponent<RawImage>();
        }

        boardImage.texture = boardTexture;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        if (shouldPlay && (playSteps > 0 || playIndefinitely)) {
            // Simulate the game of "life"
            SimulateOneStep();

            playSteps--;
        }

        playSteps = Mathf.Max(playSteps, 0);
    }

    void SimulateOneStep() {
        int adjacentLives = 0;
        for (int i = 0; i < boardArray.Count; ++i) {
            for (int j = 0; j < boardArray[i].Count; ++j) {
                // TODO: add variable rules into this
                adjacentLives = 0;

                if (i > 0)
                    adjacentLives += boardArray[i - 1][j].CurrentValue > 0 ? 1 : 0;

                if (i < boardArray.Count - 1)
                    adjacentLives += boardArray[i + 1][j].CurrentValue > 0 ? 1 : 0;

                if (j > 0)
                    adjacentLives += boardArray[i][j - 1].CurrentValue > 0 ? 1 : 0;

                if (j < boardArray.Count - 1)
                    adjacentLives += boardArray[i][j + 1].CurrentValue > 0 ? 1 : 0;

                boardArray[i][j].NextValue = adjacentLives >= 2 ? 1 : 0;
            }
        }

        // Toggle active state
        GameCellScript.State = !GameCellScript.State;
    }
}
