using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using LitJson;
using Zenject;
using System;

public class PostRequests : MonoBehaviour
{
    //////////////Fake Requests/////////////////////

    #region Fields

    [SerializeField] private string post_url, get_url;
    [Inject] Statistics stats;
    private JsonData json_data;

    #endregion

    #region Post Request Start

    public void PostRequestStart(int xnum, int ynum, int bombquantity)
    {
        GameDataStart GDS = new GameDataStart();
        GDS.grid_size = new xyvalues { x = xnum, y = ynum };
        GDS.bomb_quantity = bombquantity;
        string json_obj = JsonUtility.ToJson(GDS);
        Debug.Log(json_obj);
        StartCoroutine(PostRequestProcess(json_obj, true));
    }

    #endregion

    #region Post Request Select

    public void PostRequestSelect(string id, string status, int xnum, int ynum)
    {
        GameDataSelect GDS = new GameDataSelect();
        GDS.game_id = id;
        GDS.game_status = status;
        GDS.grid_position = new xyvalues { x = xnum, y = ynum };
        string json_obj = JsonUtility.ToJson(GDS);
        Debug.Log(json_obj);
        StartCoroutine(PostRequestProcess(json_obj, false));
    }

    #endregion

    #region Post Request Process

    private IEnumerator PostRequestProcess(string json_obj, bool start)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(post_url, json_obj))
        {
            yield return www.SendWebRequest();

            if (String.IsNullOrEmpty(www.error))
            {
                if (start)
                {
                    yield return StartCoroutine(Response());
                }
                else Debug.Log("Sent");
            }
            else Debug.Log("Connection failed");
        }
    }

    #endregion

    #region Response

    private IEnumerator Response()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(get_url))
        {
            yield return www.SendWebRequest();
            if (String.IsNullOrEmpty(www.error))
            {
                json_data = JsonMapper.ToObject(www.downloadHandler.text);
                if (json_data.Keys.Contains("game_id"))
                {
                    stats.game_id = json_data["game_id"].ToString();
                }
            }
            else Debug.Log("Connection failed");
        }
    }

    #endregion

}
public struct GameDataStart
{
    public xyvalues grid_size;
    public int bomb_quantity;
}
public struct GameDataSelect
{
    public string game_id, game_status;
    public xyvalues grid_position;
}
[Serializable]
public struct xyvalues
{
    public int x, y;
}