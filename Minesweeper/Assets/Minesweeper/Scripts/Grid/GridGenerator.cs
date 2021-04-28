using System.Collections.Generic;
using UnityEngine;
using Zenject;

public delegate void SetFlagNumber(int number);
public delegate void FloodFill(int x, int y);
public delegate void PostRequestsSelect(string id, string status, int x, int y);
public delegate void SetStatus(string stat);
public delegate string Status();
public delegate void Manager();
public delegate bool CheckBombs();

public class GridGenerator : MonoBehaviour
{
    #region Fields

    [SerializeField] private Cell cell;
    [SerializeField] private Sprite[] cell_sprites;
    [SerializeField] private int grid_w, grid_h;

    private Cell[,] grid;
    private GameObject grid_parent;
    [Inject] private GridOperations grid_operations;

    private MinesweeperManager game_manager;
    private PostRequests postrequest;
    private Statistics stats;
    private SmileButtonStates smilestates;
    private GameTimer timer;

    #endregion

    #region Inititialization

    void Start()
    {
        GenerateGrid();
    }

    [Inject]
    public void InjectDependencies(MinesweeperManager gm, PostRequests pr, Statistics st,
        SmileButtonStates smt, GameTimer gtimer)
    {
        game_manager = gm;
        postrequest = pr;
        stats = st;
        smilestates = smt;
        timer = gtimer;
    }

    #endregion

    #region Generate Grid

    private void GenerateGrid()
    {
        grid_parent = new GameObject("Grid_parent");
        grid = new Cell[grid_w, grid_h];

        InstantiateGrid();
        CalculateNearblyBombs();
        stats.SetGameStatus("ingame");
        postrequest.PostRequestStart(grid_w, grid_h, grid_operations.CalculateNumberofBombs());

        game_manager.uncover_bombs += grid_operations.UncoverAllBombs;
        game_manager.destroy_cells += grid_operations.DestroyCells;
    }

    private void InstantiateGrid()
    {
        for (int x = 0; x < grid_w; x++)
        {
            for (int y = 0; y < grid_h; y++)
            {
                grid[x, y] = Instantiate(cell, new Vector2(x, y), Quaternion.identity);
                grid[x, y].SetCell(Random.value < 0.15f, x, y);
                grid[x, y].transform.parent = grid_parent.transform;
                SetEvents(grid[x, y]);
                grid_operations.FillCellLists(grid[x, y]);
            }
        }
    }

    #endregion

    #region Set Cell Events

    private void SetEvents(Cell cell)
    {
        cell.set_flagnumber += stats.SetFlagNumber;
        cell.allowflag += stats.CheckFlagNumber;
        cell.get_gameid += stats.GetGameID;
        cell.get_status += stats.GetStatus;
        cell.set_status += stats.SetGameStatus;
        cell.activate_loss += game_manager.Loss;
        cell.activate_win += game_manager.Win;
        cell.enabletimer += timer.EnableTimer;
        cell.ifbombsfound += grid_operations.IfBombsFound;
        cell.post_request += postrequest.PostRequestSelect;
        cell.click_smile += smilestates.ClickedSmile;
    }

    #endregion

    #region Set Number Of Anjacement Bombs

    private void CalculateNearblyBombs()
    {
        int ind;
        for (int x = 0; x < grid_w; x++)
        {
            for (int y = 0; y < grid_h; y++)
            {
                ind = CalculateNumberOfAdjacementBombs(x, y);
                grid[x, y].SetRevealSprite(cell_sprites[ind]);
                if (ind == 0)
                {
                    grid[x, y].add_floodfill += FloodFillEmptyCells;
                }
            }
        }
    }

    #endregion

    #region Find Number Of Anjacement Bombs

    private int CalculateNumberOfAdjacementBombs(int x_n, int y_n)
    {
        int count = 0;
        if (CheckBomb(x_n, y_n + 1))
        {
            count++;
        }
        if (CheckBomb(x_n + 1, y_n + 1))
        {
            count++;
        }
        if (CheckBomb(x_n + 1, y_n))
        {
            count++;
        }
        if (CheckBomb(x_n + 1, y_n - 1))
        {
            count++;
        }
        if (CheckBomb(x_n, y_n - 1))
        {
            count++;
        }
        if (CheckBomb(x_n - 1, y_n - 1))
        {
            count++;
        }
        if (CheckBomb(x_n - 1, y_n))
        {
            count++;
        }
        if (CheckBomb(x_n - 1, y_n + 1))
        {
            count++;
        }
        return count;
    }

    private bool CheckBomb(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid_w && y < grid_h)
            return grid[x, y].bomb;
        else return false;
    }

    #endregion

    #region Flood Fill Algorithm To Reveal EmptyCells

    private void FloodFillEmptyCells(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid_w && y < grid_h)
        {
            if (grid[x, y].cchecked)
                return;

            if (!grid[x, y].bomb)
            {
                grid[x, y].ActivateSafeCell();
            }
            else 
                return;

            if (CalculateNumberOfAdjacementBombs(x, y) > 0)
                return;

            grid[x, y].cchecked = true;

            FloodFillEmptyCells(x - 1, y);
            FloodFillEmptyCells(x + 1, y);
            FloodFillEmptyCells(x, y - 1);
            FloodFillEmptyCells(x, y + 1);
        }
    }

    #endregion

}
