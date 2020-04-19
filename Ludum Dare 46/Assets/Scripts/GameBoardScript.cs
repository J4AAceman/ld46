using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameBoardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public bool shouldPlay = false;
    public bool playIndefinitely = false;
    public int playSteps = 0;
    public RawImage boardImage;

    //private uint _boardSize = 64;
    //public uint BoardSize {
    //    get { return _boardSize; }
    //    set {
    //        if (value >= 0 && value <= 1024)
    //            _boardSize = value;
    //    }
    //}
    [Range(1, 1024)]
    public int BoardSize = 64;

    private List<List<GameCellScript>> boardArray;
    private Texture2D boardTexture;
    private RectTransform boardRectTransform;

    void Awake() {
        // Make boardArray
        boardArray = new List<List<GameCellScript>>();
        for (var i = 0; i < BoardSize; ++i) {
            boardArray.Add(new List<GameCellScript>());
            for (var j = 0; j < BoardSize; ++j) {
                boardArray[i].Add(new GameCellScript());
            }
        }

        boardTexture = new Texture2D(BoardSize, BoardSize, TextureFormat.RGBA32, false);
        for (int x = 0; x < boardTexture.width; ++x) {
            for (int y = 0; y < boardTexture.height; ++y) {
                boardTexture.SetPixel(x, y, Color.grey);
            }
        }
        boardTexture.filterMode = FilterMode.Point;
        boardTexture.Apply();
        

        if (boardImage == null) {
            boardImage = GetComponent<RawImage>();
        }

        boardImage.texture = boardTexture;
        boardImage.material.mainTexture = boardTexture;

        boardRectTransform = gameObject.GetComponent<RectTransform>();
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

                boardArray[i][j].NextValue = adjacentLives == 2 ? 1 : 0;

                // TODO: some sort of translation from value to a color
                boardTexture.SetPixel(i, j, boardArray[i][j].NextValue > 0 ? Color.black : Color.grey);
            }
        }
        boardTexture.Apply();

        // Toggle active state
        GameCellScript.State = !GameCellScript.State;
    }

    public void OnPointerDown(PointerEventData eventData) {
        //throw new System.NotImplementedException();
        //Debug.Log("Pointer Down " + eventData.position.ToString());
        SetGameCell(eventData);
    }

    public void OnPointerUp(PointerEventData eventData) {
        //throw new System.NotImplementedException();
        //Debug.Log("Pointer Up " + eventData.position.ToString());
    }

    public void OnDrag(PointerEventData eventData) {
        SetGameCell(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        SetGameCell(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {

    }

    public void Reset() {
        shouldPlay = false;
        // TODO: reset
    }

    public void Pause() {
        shouldPlay = false;
    }

    public void Step( int cnt ) {
        playSteps = cnt;
        shouldPlay = true;
    }

    public void Play() {
        playIndefinitely = true;
        shouldPlay = true;
        // TODO: playback rate
    }

    public void FastForward() {
        playIndefinitely = true;
        shouldPlay = true;
        // TODO: playback rate
    }

    public void SuperFastForward() {
        playIndefinitely = true;
        shouldPlay = true;
        // TODO: playback rate
    }

    private void SetGameCell(PointerEventData eventData) {
        Vector2 localPoint;
        int x, y;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(boardRectTransform, eventData.pointerCurrentRaycast.screenPosition, eventData.pressEventCamera, out localPoint);

        x = Mathf.FloorToInt((localPoint.x / boardRectTransform.rect.width) * BoardSize) + (BoardSize / 2);
        y = Mathf.FloorToInt((localPoint.y / boardRectTransform.rect.height) * BoardSize) + (BoardSize / 2);

        boardTexture.SetPixel(x, y, Color.black);
        boardTexture.Apply();

        ////////////////////////////////////////
        // TODO: link game board to texture
        ////////////////////////////////////////
        boardArray[x][y].CurrentValue = 1;
    }

}
