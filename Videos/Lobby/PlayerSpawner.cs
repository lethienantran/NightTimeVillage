using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    int index = 0;

    PhotonView photonView;

    PhotonView _playerPhotonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                photonView.RPC("InstantiationPlayer", player, index);
                index++;
            }
        }
    }


    [PunRPC]
    void InstantiationPlayer(int index)
    {

        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null)
        {
            GameObject playerToSpawn = playerPrefabs[0];
            GameObject newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoints[index].position, Quaternion.identity);
            _playerPhotonView = newPlayer.GetComponent<PhotonView>();
        }
        else
        {
            GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            GameObject newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoints[index].position, Quaternion.identity);
            _playerPhotonView = newPlayer.GetComponent<PhotonView>();
        }
    }

    public void DestroyPlayer()
    {
        PhotonNetwork.Destroy(_playerPhotonView);
    }


}
