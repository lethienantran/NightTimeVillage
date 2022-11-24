using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class VotingManager : MonoBehaviourPunCallbacks
{
    public GameObject voteButtonUI;
    public GameObject VoteWindow;
    [SerializeField] private Button _skipVoteButton;

    [SerializeField] private PlayerSpawner _network;

    public static VotingManager Instance;

    [HideInInspector] public PhotonView view;

    [SerializeField] private VotePlayerItem _votePlayerItemPrefab;
    [SerializeField] private Transform _votePlayerItemContainer;
    [HideInInspector] private bool HasAlreadyVoted;

    private List<int> _playerThatVotedList = new List<int>();
    private List<int> _playerThatHaveBeenVotedList = new List<int>();
    private List<int> _playerThatHaveBeenRemovedList = new List<int>();

    private List<VotePlayerItem> _votePlayerItemList = new List<VotePlayerItem>();

    [SerializeField] private GameObject _removePlayerWindow;
    [SerializeField] private GameObject _removedPlayerWindow;
    [SerializeField] private Text _removePlayerText;

    private void Awake()
    {
        Instance = this;
    }


    private void ToggleAllButtons(bool areActive)
    {
        _skipVoteButton.interactable = areActive;
        foreach (VotePlayerItem votePlayerItem in _votePlayerItemList)
        {
            votePlayerItem.ToggleButton(areActive);

        }
    }

    public void CastVote(int targetActorNumber)
    {
        if (HasAlreadyVoted) { return; }

        HasAlreadyVoted = true;

        ToggleAllButtons(false);
        view.RPC("RPC_CastVote", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, targetActorNumber);
    }

    [PunRPC]
    public void RPC_CastVote(int actorNumber, int targetActorNumber)
    {
        int remainingPlayers = PhotonNetwork.CurrentRoom.PlayerCount - _playerThatHaveBeenRemovedList.Count; //should be PhotonNetwork.currentroom.playercount - playerdeadlist.count;

        foreach (VotePlayerItem votePlayerItem in _votePlayerItemList)
        {
            if (votePlayerItem.ActorNumber == actorNumber)
            {
                votePlayerItem.UpdateStatus(targetActorNumber == -1 ? "SKIPPED" : "VOTED");
            }
        }

        //Update player who already vote and update player being vote.
        if (!_playerThatVotedList.Contains(actorNumber))
        {
            _playerThatVotedList.Add(actorNumber);
            _playerThatHaveBeenVotedList.Add(targetActorNumber);
        }

        if (!PhotonNetwork.IsMasterClient) { return; }
        if (_playerThatVotedList.Count < remainingPlayers) { return; }

        //Count  all the votes
        Dictionary<int, int> playerVoteCount = new Dictionary<int, int>();

        foreach (int votedPlayer in _playerThatHaveBeenVotedList)
        {
            if (!playerVoteCount.ContainsKey(votedPlayer))
            {
                playerVoteCount.Add(votedPlayer, 0);
            }

            playerVoteCount[votedPlayer]++;
        }

        //Get the most voted player
        int mostVotedPlayer = -1;
        int mostVotes = int.MinValue;

        foreach (KeyValuePair<int, int> playerVote in playerVoteCount)
        {
            if (playerVote.Value > mostVotes)
            {
                mostVotes = playerVote.Value;
                mostVotedPlayer = playerVote.Key;
            }
        }

        //End the voting session
        if (mostVotes >= remainingPlayers / 2)
        {
            //Remove the player from the village
            view.RPC("RPC_RemovePlayer", RpcTarget.All, mostVotedPlayer);
        }
    }

    void Start()
    {
        view = PhotonView.Get(this);
    }

    void Update()
    {
        if(DayNightCycle.hours >= 7 && DayNightCycle.hours < 9)
        {
            voteButtonUI.SetActive(true);
        }
        else if(DayNightCycle.hours >= 9)
        {
            voteButtonUI.SetActive(false);
            if (!HasAlreadyVoted)
            {
                CastVote(-1);
            }
        }
    }

    public void Voting()
    {
        VoteWindow.SetActive(true);
        view.RPC("RPC_Voting", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_Voting()
    {
        _playerThatVotedList.Clear();
        _playerThatHaveBeenVotedList.Clear();
        HasAlreadyVoted = false;
        PopulatePlayerList();
        ToggleAllButtons(true);
    }

    private void PopulatePlayerList()
    {
        for (int i = 0; i < _votePlayerItemList.Count; i++)
        {
            Destroy(_votePlayerItemList[i].gameObject);
        }
        _votePlayerItemList.Clear();

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                continue;
            }

            if (_playerThatHaveBeenRemovedList.Contains(player.Value.ActorNumber))
            {
                continue;
            }

            VotePlayerItem newPlayerItem = Instantiate(_votePlayerItemPrefab, _votePlayerItemContainer);
            newPlayerItem.Initialize(player.Value, this);
            _votePlayerItemList.Add(newPlayerItem);
        }
    }

    [PunRPC]
    public void RPC_RemovePlayer(int actorNumber)
    {
        

        StartCoroutine(FadeRemovePlayerWindow(actorNumber));

    }

    private IEnumerator FadeRemovePlayerWindow(int actorNumber)
    {
        VoteWindow.SetActive(false);
        string playerName = string.Empty;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == actorNumber)
            {
                playerName = player.Value.NickName;
                break;
            }
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            _removedPlayerWindow.SetActive(true);
            //_removedPlayerWindow.SetActive(true);

        }
        else
        {
            _removePlayerWindow.SetActive(true);
            _removePlayerText.text = actorNumber == -1 ? "No One has been removed" : "Player " + playerName + " has been removed";
        }

        yield return new WaitForSeconds(3);
        _removePlayerWindow.SetActive(false);

        if(PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            if(PlayerController.isKidnap){
                GameplayManager.numberOfKidnapper -= 1;
            }
            _network.DestroyPlayer();
            //_removedPlayerWindow.SetActive(true);

        }
    }
}
