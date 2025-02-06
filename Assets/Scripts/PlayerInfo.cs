


using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerInfo Instance;
    public Players _Player = Players.None;
    [SerializeField] GameObject Player1, Player2;
    public bool PlayerInput = false;

    public int PlayerCount1 = 0, PlayerCount2 = 0;
    public int WinningCount = 3;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        DetectClick.CheckWinngCondition += CheckWinnigCondition;
        GameUIControl.Instance.DestroyorRefreshGrid += DestroyorRefreshGrid;


    }
    void Start()
    {
        _Player = Players.Player1;
        Player2.GetComponent<CanvasGroup>().alpha = 0.5f;
    }


    void OnDisable()
    {
        DetectClick.CheckWinngCondition -= CheckWinnigCondition;
        GameUIControl.Instance.DestroyorRefreshGrid -= DestroyorRefreshGrid;

    }

    void DestroyorRefreshGrid(bool IsRefresh)
    {
        //if (IsRefresh)
        {
            PlayerInput = false;
            Player1.GetComponent<CanvasGroup>().alpha = 1f;
            Player2.GetComponent<CanvasGroup>().alpha = 0.5f;
            _Player = Players.Player1;
            PlayerCount1 = 0;
            PlayerCount2 = 0;

            Player1.GetComponent<Image>().color = Color.white;
            Player2.GetComponent<Image>().color = Color.white;
        }
    }



    private void CheckWinnigCondition(Vector2Int _Pos)
    {
        bool isPlayer1 = (_Player == Players.Player1);
        GridManager.Instance.UpdateDetails(_Pos, isPlayer1);

        // Check if the current player has met the winning condition
        if (CheckBlocksForWinCondition(_Pos, isPlayer1) == WinningCount)
        {
            Debug.LogError("==>| " + (isPlayer1 ? "Player1 Wins" : "Player2 Wins") + "::" + WinningCount);
            Image winningPlayerImage = isPlayer1 ? Player1.GetComponent<Image>() : Player2.GetComponent<Image>();
            winningPlayerImage.color = Color.green;
        }
        else
        {
            // Switch to the next player
            SwitchPlayer(isPlayer1 ? Players.Player2 : Players.Player1);
        }
    }

    private void SwitchPlayer(Players nextPlayer)
    {
        _Player = nextPlayer;

        // Swap the alpha values of the player indicators
        float player1Alpha = Player1.GetComponent<CanvasGroup>().alpha;
        Player1.GetComponent<CanvasGroup>().alpha = Player2.GetComponent<CanvasGroup>().alpha;
        Player2.GetComponent<CanvasGroup>().alpha = player1Alpha;
    }

    // Check blocks in horizontal, vertical, and diagonal directions for a winning condition
    private int CheckBlocksForWinCondition(Vector2Int parentBlockPos, bool isPlayer1)
    {
       
        Vector2Int[] directions = {
        new Vector2Int(1, 0),  // Horizontal
        new Vector2Int(0, 1),  // Vertical
        new Vector2Int(1, 1),  // Diagonal (top-left to bottom-right)
        new Vector2Int(1, -1)  // Diagonal (bottom-left to top-right)
    };

        foreach (var direction in directions)
        {
            int count = 1; // Start with the current block

            // Check in the positive direction
            count += CountConsecutiveBlocks(parentBlockPos, direction, isPlayer1);

            // Check in the negative direction
            count += CountConsecutiveBlocks(parentBlockPos, -direction, isPlayer1);

            // If the count meets the winning condition, return
            if (count >= WinningCount)
            {
                return count;
            }
        }

        return 0; // No winning condition met
    }

    // Count consecutive blocks in a given direction
    private int CountConsecutiveBlocks(Vector2Int startPos, Vector2Int direction, bool isPlayer1)
    {
        int count = 0;
        Vector2Int currentPos = startPos + direction;

        while (GridManager.Instance.CheckValidposition(currentPos.x, currentPos.y))
        {
            Blockinfo blockInfo = GridManager.Instance.IsBlockFilled(currentPos.x, currentPos.y);

            if (!blockInfo.IsBlockFree && blockInfo.IsPlayer1Occupied == isPlayer1)
            {
                count++;
                currentPos += direction;
            }
            else
            {
                break;
            }
            Debug.Log("==>:" +blockInfo.IsPlayer1Occupied +"::" +count);
        }

        return count;
    }


}

public enum Players
{
    Player1,
    Player2,
    None
}
