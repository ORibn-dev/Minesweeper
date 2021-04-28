using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameTimer : MonoBehaviour
{
    #region Fields

    [HideInInspector] public bool allowtimer;
    [SerializeField] private Text TimerUI;
    [Inject] private MinesweeperManager minesweepermanager;

    private float timer;

    #endregion

    #region Intitialization

    void Start()
    {
        minesweepermanager.disabletimer += DisableTimer;
    }

    #endregion

    #region Enable Or Disable Timer

    public void EnableTimer()
    {
        if (!allowtimer)
        {
            allowtimer = true;
        }

    }
    public void DisableTimer()
    {
        allowtimer = false;
    }

    #endregion

    #region Process

    void Update()
    {
        if (allowtimer)
        {
            timer += Time.deltaTime;
            TimerUI.text = (int)timer + "";
        }
    }

    #endregion

}
