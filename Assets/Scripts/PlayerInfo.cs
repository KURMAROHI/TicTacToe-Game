
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerInfo Instance;
    public Players _Player = Players.None;
    [SerializeField] GameObject Player1, Player2;

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
    }
    void Start()
    {
        _Player = Players.Player1;
        Player2.GetComponent<CanvasGroup>().alpha = 0.5f;
    }


    void OnDisable()
    {
        DetectClick.CheckWinngCondition -= CheckWinnigCondition;
    }


    private void CheckWinnigCondition(GameObject ParentBlock)
    {
        //        Debug.Log("==>Checking Winnign Condition");
        Vector2Int _Pos = GridManager.Instance.PosofBlock(ParentBlock);
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

        if (CheckblocksForWinCondtion(_Pos, ISplayer1) == WinningCount)
        {
            Debug.LogError("==>| " + (ISplayer1 ? "Player1 Wins" : "Player 2 Wins"));
        }
        else
        {
            int NormalCounter = 0;
            SwitchPlayer(ref NormalCounter, ISplayer1 ? Players.Player2 : Players.Player1);
        }
    }

    private void SwitchPlayer(ref int playerCount, Players nextPlayer)
    {
        playerCount++;
        // currentPlayerCanvas.alpha = 0.5f;
        // nextPlayerCanvas.alpha = 1f;
        float Player1alpha = Player1.GetComponent<CanvasGroup>().alpha;
        Player1.GetComponent<CanvasGroup>().alpha = Player2.GetComponent<CanvasGroup>().alpha;
        Player2.GetComponent<CanvasGroup>().alpha = Player1alpha;
        _Player = nextPlayer;
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
                            for (int j = 0; j < WinningCount - 1; j++)
                            {
                                Vector2Int Newpos1 = NewCheckPos + Possiblemove;
                                if (ISFailedInSameDirection)
                                {
                                    Possiblemove = new Vector2Int(-Possiblemove.x, -Possiblemove.y);
                                    Newpos1 = ParentBlockpos + Possiblemove;
                                }


                                if (GridManager.Instance.CheckValidposition(Newpos1.x, Newpos1.y))
                                {
                                    Blockinfo blockinfo1 = GridManager.Instance.IsBlockFilled(Newpos1.x, Newpos1.y);
                                    if (!blockinfo1.IsBlockFree && blockinfo1.IsPlayer1Occupied == ISplayer1)
                                    {
                                        Count++;
                                        break;
                                    }
                                    else
                                    {
                                        ISFailedInSameDirection = true;
                                    }
                                }



                            }

                            if (Count == WinningCount)
                            {
                                Debug.Log("==>Count|" + Count + "::" + i);
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
