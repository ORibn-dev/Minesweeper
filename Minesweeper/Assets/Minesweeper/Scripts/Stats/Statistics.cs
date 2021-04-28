using UnityEngine;
using UnityEngine.UI;
using System;

public class Statistics : MonoBehaviour
{
    #region Fields

    [SerializeField] private Text FlagNumberUI;
    [SerializeField] private int flagnumber;
    [HideInInspector] public string game_id;

    private string game_status;
    private int bomb_quantity;

    #endregion

    #region Inititialization

    void Start()
    {
        GenerateGameID();
    }

    public void GenerateGameID()
    {
        game_id = "Minesweeper - " + Environment.UserName + " " + Environment.MachineName 
            + " " + DateTime.Now;
    }

    public void SetGameStatus(string status)
    {
        game_status = status;
    }

    #endregion

    #region Get Data

    public string GetStatus()
    {
        return game_status;
    }

    public string GetGameID()
    {
        return game_id;
    }

    #endregion

    #region Number Of Flags

    public bool CheckFlagNumber()
    {
        if (flagnumber > 0)
        {
            return true;
        }
        else return false;
    }
    public void SetFlagNumber(int number)
    {
        if (flagnumber >= 0)
        {
            flagnumber += number;
            FlagNumberUI.text = flagnumber.ToString();
        }
    }

    #endregion
}
