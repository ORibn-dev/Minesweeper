using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Fields

    [HideInInspector] public bool bomb, cchecked, flagged;
    [SerializeField] private Sprite FlagSprite, BombSprite, DifBombSprite, ExplodedBomb;

    private SpriteRenderer Cell_Renderer;
    private Sprite BlockSprite, RevealSprite;
    private bool exploded, revealed;
    private int x_pos, y_pos;

    #endregion

    #region Events

    public event SetFlagNumber set_flagnumber;
    public event Manager click_smile, activate_loss, activate_win, enabletimer;
    public event FloodFill add_floodfill;
    public event CheckBombs ifbombsfound, allowflag;
    public event PostRequestsSelect post_request;
    public event Status get_status, get_gameid;
    public event SetStatus set_status;

    #endregion

    #region Button Actions

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RevealCell();
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetFlag();
        }
    }

    #endregion

    #region Set Cell

    public void SetCell(bool ifbomb, int x, int y)
    {
        Cell_Renderer = GetComponent<SpriteRenderer>();
        BlockSprite = Cell_Renderer.sprite;
        bomb = ifbomb;
        x_pos = x;
        y_pos = y;
    }

    public void SetRevealSprite(Sprite revsprite)
    {
        RevealSprite = revsprite;
    }

    #endregion

    #region Reveal Cell

    private void RevealCell()
    {
        if (!flagged && !revealed)
        {
            if (!bomb)
            {
                ActivateSafeCell();
                click_smile();
                CheckBombs();
                enabletimer();
            }
            else
            {
                ActivateBombCell();
            }
        }
        if (add_floodfill != null)
        {
            add_floodfill(x_pos, y_pos);
        }
    }

    public void ActivateSafeCell()
    {
        Cell_Renderer.sprite = RevealSprite;
        revealed = true;
    }

    public void ActivateBombCell()
    {
        exploded = true;
        set_status("loss");
        post_request(get_gameid(), get_status(), x_pos, y_pos);
        activate_loss();
    }

    #endregion

    #region Set Flag

    private void SetFlag()
    {
        if (!revealed && allowflag())
        {
            enabletimer();
            if (!flagged)
            {
                Flag(FlagSprite, -1, true);
            }
            else Flag(BlockSprite, 1, false);
        }
    }

    private void Flag(Sprite spr, int num, bool flag)
    {
        Cell_Renderer.sprite = spr;
        set_flagnumber(num);
        flagged = flag;
    }

    #endregion

    #region Check Or Reveal Bombs

    private void CheckBombs()
    {
        if (ifbombsfound())
        {
            set_status("win");
            post_request(get_gameid(), get_status(), x_pos, y_pos);
            activate_win();
        }
        else post_request(get_gameid(), get_status(), x_pos, y_pos);
    }

    public void RevealBomb()
    {
        if (exploded)
        {
            Cell_Renderer.sprite = ExplodedBomb;
        }
        else if (flagged)
        {
            Cell_Renderer.sprite = DifBombSprite;
        }
        else Cell_Renderer.sprite = BombSprite;
    }

    #endregion

}
