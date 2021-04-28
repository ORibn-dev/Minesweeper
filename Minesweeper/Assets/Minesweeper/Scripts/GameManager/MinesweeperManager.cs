using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void CellOperations();
public delegate void CreateScreenshot();
public delegate void SmileStates(int ind);

public class MinesweeperManager : MonoBehaviour
{
    #region Events

    public event CellOperations uncover_bombs, destroy_cells, disabletimer;
    public event SmileStates set_smile;
    public event CreateScreenshot create_screenshot;

    #endregion

    #region Restart

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region EndGame

    public void Win()
    {
        SetEndingState(2);
        create_screenshot();
    }

    public void Loss()
    {
        SetEndingState(3);
    }

    private void SetEndingState(int ind)
    {
        set_smile(ind);
        disabletimer();
        uncover_bombs();
        destroy_cells();
    }

    #endregion
}
