using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SaveScreenshot : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator ScreenshotNotifUI;
    [Inject] private MinesweeperManager minesweepermanager;
    private Image ScreenshotHolder;

    #endregion

    #region Initialization

    void Start()
    {
        ScreenshotHolder = ScreenshotNotifUI.gameObject.GetComponent<Image>();
        minesweepermanager.create_screenshot += Notification;
    }

    #endregion

    #region Notification

    private void Notification()
    {
        TakeScreenshot();
        ScreenshotNotifUI.gameObject.SetActive(true);
        ScreenshotNotifUI.Play("ScreenshotNotif");
    }

    #endregion

    #region Take Screenshot

    private void TakeScreenshot()
    {
        GleyShare.Manager.CaptureScreenshot(ScreenshotCaptured);
    }
    private void ScreenshotCaptured(Sprite sprite)
    {
        if (sprite != null)
        {
            ScreenshotHolder.sprite = sprite;
        }
    }

    #endregion

    #region Share

    public void Share()
    {
        Debug.Log("Shared!");
        GleyShare.Manager.SharePicture();
        /////////for IOS and Android////////////
    }

    #endregion
}
