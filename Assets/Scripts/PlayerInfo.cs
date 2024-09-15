

using System.Collections.Generic;
using DG.Tweening.Core.Easing;
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
        //        Debug.Log("==>Checking Winnign Condition");
        //Vector2Int _Pos = GridManager.Instance.PosofBlock(ParentBlock);
        bool ISplayer1 = false;
        if (_Player == Players.Player1)
        {
            GridManager.Instance.UpdateDetails(_Pos, true);
            ISplayer1 = true;
            if (PlayerCount1 <= WinningCount - 2)
            {
                SwitchPlayer(ref PlayerCount1, Players.Player2);
                return;
            }
        }
        else
        {
            GridManager.Instance.UpdateDetails(_Pos);
            if (PlayerCount2 <= WinningCount - 2)
            {
                SwitchPlayer(ref PlayerCount2, Players.Player1);
                return;
            }
        }




        int Count = CheckblocksForWinCondtion(_Pos, ISplayer1);
        if (Count == WinningCount)
        {
            Debug.LogError("==>| " + (ISplayer1 ? "Player1 Wins" : "Player 2 Wins") + "::" + WinningCount);
            if (ISplayer1)
            {
                Player1.GetComponent<Image>().color = Color.green;
            }
            else
            {
                Player2.GetComponent<Image>().color = Color.green;
            }
        }
        else
        {
            Debug.LogError("==>Count|" + Count);
            int NormalCounter = 0;
            SwitchPlayer(ref NormalCounter, ISplayer1 ? Players.Player2 : Players.Player1);
        }
    }

    private void SwitchPlayer(ref int playerCount, Players nextPlayer, bool SwitchPlayer = true)
    {
        playerCount++;
        float Player1alpha = Player1.GetComponent<CanvasGroup>().alpha;
        Player1.GetComponent<CanvasGroup>().alpha = Player2.GetComponent<CanvasGroup>().alpha;
        Player2.GetComponent<CanvasGroup>().alpha = Player1alpha;
        if (SwitchPlayer)
        {
            _Player = nextPlayer;
        }
    }


    // tghsi Fuinction used to Check the blcoks in horizantal and and Diagniol for Winning Condition
    int CheckblocksForWinCondtion(Vector2Int ParentBlockpos, bool ISplayer1)
    {
        List<Vector2Int> LstofPossibleMoves = new List<Vector2Int>{
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,-1),
            new Vector2Int(-1,1),
        };



        int Count = 1;
        Vector2Int NewCheckPos = Vector2Int.zero, Possiblemove = Vector2Int.zero;
        //  try
        {
            if (Count == 1)
            {
                for (int i = 0; i < LstofPossibleMoves.Count; i++)
                {
                    Vector2Int Newpos = ParentBlockpos + LstofPossibleMoves[i];
                    // Debug.Log("==>1|" + ParentBlockpos + "::" + LstofPossibleMoves[i] + "::" + Newpos);
                    if (GridManager.Instance.CheckValidposition(Newpos.x, Newpos.y))
                    {
                        Blockinfo blockinfo = GridManager.Instance.IsBlockFilled(Newpos.x, Newpos.y);
                        if (!blockinfo.IsBlockFree && blockinfo.IsPlayer1Occupied == ISplayer1)
                        {
                            Possiblemove = LstofPossibleMoves[i];
                            NewCheckPos = Newpos;
                            Count++;


                            #region  Checking Furthur After getting one Block 
                            bool ISFailedInSameDirection = false;
                            bool IsCheckingOtherDir = false;
                            for (int j = 0; j < WinningCount - 1; j++)
                            {
                                Vector2Int Newpos1 = NewCheckPos + Possiblemove;
                                Debug.LogError("==>1" + ParentBlockpos + "::" + Newpos1 + "::" + NewCheckPos + "::" + Possiblemove + "::" + Count);
                                if (ISFailedInSameDirection)
                                {
                                    ISFailedInSameDirection = false;
                                    IsCheckingOtherDir = true;
                                    Possiblemove = new Vector2Int(-Possiblemove.x, -Possiblemove.y);
                                    Newpos1 = ParentBlockpos + Possiblemove;
                                }

                                NewCheckPos = Newpos1;
                                if (GridManager.Instance.CheckValidposition(Newpos1.x, Newpos1.y))
                                {
                                    Blockinfo blockinfo1 = GridManager.Instance.IsBlockFilled(Newpos1.x, Newpos1.y);
                                    Debug.LogError("==>2" + ParentBlockpos + "::" + Newpos1 + "::" + NewCheckPos + "::" + Possiblemove + "::" + Count);
                                    if (!blockinfo1.IsBlockFree && blockinfo1.IsPlayer1Occupied == ISplayer1)
                                    {

                                        Count++;
                                        // Debug.Log("==>Count at break|" + Count);
                                        if (Count == WinningCount)
                                        {
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        Debug.LogError("==>IsFailed|" + IsCheckingOtherDir);
                                        ISFailedInSameDirection = true;
                                        if (IsCheckingOtherDir)
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    ISFailedInSameDirection = true;
                                    if (IsCheckingOtherDir)
                                    {
                                        break;
                                    }

                                }



                            }

                            if (Count == WinningCount)
                            {
                                //Debug.Log("==>Count|" + Count + "::" + i);
                                return Count;
                            }
                            else
                            {
                                Count = 1;
                            }
                            #endregion
                        }

                    }
                }
            }


        }
        // catch (Exception e)
        // {
        //     Debug.LogError("==>" + e.Message);
        // }

        return Count;
    }


}

public enum Players
{
    Player1,
    Player2,
    None
}
