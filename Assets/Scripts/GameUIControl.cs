using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameUIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameUIControl Instance;
    [SerializeField] GameObject GridSelectionControl;
    public GameObject Loading;
    public UnityAction<bool> DestroyorRefreshGrid;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {

    }

    public void ORefreShButtonClick()
    {
        Debug.Log("==>on refresh buton Click");
        DestroyorRefreshGrid?.Invoke(true);
    }

    public void OnBackButtonPresse()
    {
        GridSelectionControl.SetActive(true);
        DestroyorRefreshGrid?.Invoke(false);
    }
}


[System.Serializable]
public struct GridDetails
{
    public GameObject Block;
    public Vector2Int Blockpos;

    public GridDetails(GameObject item, Vector2Int pos)
    {
        this.Block = item;
        this.Blockpos = pos;
    }
}

[System.Serializable]
public struct Blockinfo
{
    public bool IsBlockFree;
    public bool IsPlayer1Occupied;
}
