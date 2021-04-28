using System.Collections.Generic;
using UnityEngine;

public class GridOperations
{
    #region Inititalization

    private List<Cell> bombs, allcells;

    public GridOperations()
    {
        bombs = new List<Cell>();
        allcells = new List<Cell>();
    }

    #endregion

    #region Cell Operations

    public void FillCellLists(Cell cell)
    {
        allcells.Add(cell);
        if (cell.bomb)
        {
            bombs.Add(cell);
        }
    }

    public void DestroyCells()
    {
        foreach (Cell cell in allcells)
        {
            GridGenerator.Destroy(cell);
        }
    }

    #endregion

    #region Bomb Operations

    public int CalculateNumberofBombs()
    {
        return bombs.Count;
    }

    public bool IfBombsFound()
    {
        foreach (Cell cell in bombs)
        {
            if (!cell.flagged)
            {
                return false;
            }
        }
        return true;
    }

    public void UncoverAllBombs()
    {
        foreach (Cell cell in bombs)
        {
            cell.RevealBomb();
        }
    }

    #endregion
}
