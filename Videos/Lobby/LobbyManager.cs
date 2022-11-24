using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //Room Creation
    public InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Text roomName;
    public GameObject playButton;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    private bool isInRoom = false;
    private int currentPlayerAvatarIndex;
    public GameObject[] playerPrefabs;

    //Local Player Home Control
    public CameraControllerHome cameraHome;
    public bool playerCreated = false; 

    public HomeCharacterSelection homeCharacterSelection;
    GameObject playerHome;
    public Transform homeSpawnPoint;
    public Transform playerCurrentTrans;

    private Vector3 playerCurrentPos;

    public GameObject playerDisplayUI;
    public Text playerNameUI;

    public GameObject CharacterSelectionUI;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
        GetAppearance();
        playerNameUI.text = PhotonNetwork.NickName;

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.Tab) && !isInRoom)
        {
            if (!lobbyPanel.activeSelf)
            {
                lobbyPanel.SetActive(true);
            }
            else
            {
                lobbyPanel.SetActive(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.O) && !isInRoom)
        {
            if (!CharacterSelectionUI.activeSelf)
            {
                CharacterSelectionUI.SetActive(true);
            }
            else
            {
                CharacterSelectionUI.SetActive(false);
            }
        }

        //Change appearance of Home Player and spawn new homeplayer prefabs
        if (homeCharacterSelection.isHomeCharacterSkinSelected)
        {
            playerCurrentPos = new Vector3(playerCurrentTrans.transform.position.x, playerCurrentTrans.transform.position.y, playerCurrentTrans.transform.position.z);
            Destroy(playerHome.gameObject);
            GetPlayerCurrentAppearance();
            homeCharacterSelection.isHomeCharacterSkinSelected = false;
        }
    }

    //Room Management
    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 3, BroadcastPropsChangeToAll = true });
        }
    }

    public override void OnJoinedRoom()
    {
        playerDisplayUI.SetActive(false);
        isInRoom = true;
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        Destroy(playerHome.gameObject);
        UpdatePlayerList();

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        isInRoom = false;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        playerDisplayUI.SetActive(true);
        GetAppearance();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }
            playerItemsList.Add(newPlayerItem);

        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("GamePlay");
    }

    //Chat


    //GetAppearance to spawn home player at initial spawn point with appearance 
    public void GetAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Player Appearance Data Found!");
        if (result.Data != null && result.Data.ContainsKey("playerSkin"))
        {
            currentPlayerAvatarIndex = int.Parse(result.Data["playerSkin"].Value);
            InstantiateHomePlayer(currentPlayerAvatarIndex);
        }
        else
        {
            Debug.Log("Player Appearance Data Could Not Be Found!");
        }
    }

    //GetPlayerCurrentAppearance to spawn home player at the current spawn point with new appearance
    public void GetPlayerCurrentAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnReceived, OnError);
    }

    void OnReceived(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("playerSkin"))
        {
            currentPlayerAvatarIndex = int.Parse(result.Data["playerSkin"].Value);
            InstantiateNewHomePlayer(currentPlayerAvatarIndex);
        }
        else
        {
            Debug.Log("Player Appearance Data Could Not Be Found!");
        }
    }

    void OnError(PlayFabError error)
    {

    }

    //Instantiate Home Player at initial point
    public void InstantiateHomePlayer(int prefabsIndex)
    {
        GameObject playerToSpawn = playerPrefabs[prefabsIndex];
        playerHome = Instantiate(playerToSpawn, homeSpawnPoint.position, Quaternion.identity);
        playerCurrentTrans = playerHome.transform;
        cameraHome.startFindingTarget(playerHome);
        playerCreated = true;
    }

    //Instantiate new home player at the current position to update avatar
    public void InstantiateNewHomePlayer(int prefabsIndex)
    {
        GameObject playerToSpawn = playerPrefabs[prefabsIndex];
        playerHome = Instantiate(playerToSpawn, playerCurrentPos, Quaternion.identity);
        playerCurrentTrans = playerHome.transform;
        cameraHome.startFindingTarget(playerHome);
        playerCreated = true;

    }
}
