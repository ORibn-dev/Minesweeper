using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SmileButtonStates : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image gamestate_button;
    [SerializeField] private Sprite[] gamestate_sprites;
    [Inject] private MinesweeperManager minesweepermanager;
    private float smile_speed = 0.15f;

    #endregion

    #region Inititialization

    void Start()
    {
        minesweepermanager.set_smile += SetSmile;
    }

    #endregion

    #region Smile Methods

    private void SetSmile(int indx)
    {
        smile_speed = 0;
        gamestate_button.sprite = gamestate_sprites[indx];
    }

    public void ClickedSmile()
    {
        StartCoroutine(ClickedSmileProcess());
    }
    private IEnumerator ClickedSmileProcess()
    {
        yield return new WaitForSeconds(smile_speed);
        gamestate_button.sprite = gamestate_sprites[1];
        yield return new WaitForSeconds(smile_speed);
        gamestate_button.sprite = gamestate_sprites[0];
    }

    #endregion

}
