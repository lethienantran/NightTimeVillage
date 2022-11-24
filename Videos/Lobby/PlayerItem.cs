using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerItem : MonoBehaviourPunCallbacks
{

    public Text playerName;
    Image backgroundImage;
    public Color highlightColor;
    public GameObject LeftArrowButton;
    public GameObject RightArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public Image playerAvatar;
    public Sprite[] avatars;

    //int currentPlayerAvatarIndex = 0; bc never used

    Player player;

    public void SaveAppearance()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"playerSkin", playerProperties["playerAvatar"].ToString()},
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }
    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Player Appearance Saved! Player Skin: " + (int)playerProperties["playerAvatar"]);
    }

    public void GetAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Player Appearance Data Found!");
        if (result.Data != null && result.Data.ContainsKey("playerSkin"))
        {
            playerProperties["playerAvatar"] = int.Parse(result.Data["playerSkin"].Value);
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
        else
        {
            Debug.Log("Player Appearance Data Could Not Be Found!");
        }
    }

    void OnError(PlayFabError error)
    {

    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        playerName.color = highlightColor;
        LeftArrowButton.SetActive(true);
        RightArrowButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        SaveAppearance();
    }

    public void OnClickRightArrow()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        SaveAppearance();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable playerProperties)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
