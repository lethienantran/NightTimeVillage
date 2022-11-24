using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject introScreen;
    [SerializeField] private Text introText;
    [SerializeField] private Text introTitle;
    public static bool isShowingIntro = false;

    PhotonView pview;

    public GameObject ChatUI;

    public GameObject gameplayInfo;
    public Text roleText;

    public static int numberOfKidnapper = 0;
    void Start()
    {
        gameplayInfo.SetActive(false);
        pview = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            Initialize();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (!ChatUI.activeSelf)
            {
                ChatUI.SetActive(true);
            }
            else
            {
                ChatUI.SetActive(false);
            }
        }
        Debug.Log(numberOfKidnapper);
    }

    public void Initialize()
    {
        StartCoroutine(pickRole());
    }

    private IEnumerator pickRole()
    {
        GameObject[] players;

        List<int> playerIndex = new List<int>();
        int tries = 0;
        int kidnapNumber = 0;
        int kidnapFinalNumber;
        do
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            tries++;
            yield return new WaitForSeconds(0.1f);
        } while (players.Length < PhotonNetwork.CurrentRoom.PlayerCount && tries < 5);

        
        for(int i = 0; i < players.Length; i++)
        {
            playerIndex.Add(i);
        }
        if (players.Length < 5)
        {
            kidnapNumber = 1;
        }
        else
        {
            kidnapNumber = 2;
        }

        kidnapFinalNumber = kidnapNumber;

        while(kidnapNumber > 0)
        {
            int pickedKidnapIndex = playerIndex[Random.Range(0, playerIndex.Count)];
            playerIndex.Remove(pickedKidnapIndex);

            PhotonView view = players[pickedKidnapIndex].GetComponent<PhotonView>();
            view.RPC("RPC_SetKidnap", RpcTarget.All);
            kidnapNumber--;
        }
        pview.RPC("RPC_GameInfo", RpcTarget.All, kidnapFinalNumber);
    }

    [PunRPC]
    public void RPC_GameInfo(int kidnapFinalNumber)
    {

        StartCoroutine(ShowRoleInfoAnimation(kidnapFinalNumber));
    }

    private IEnumerator ShowRoleInfoAnimation(int kidnapFinalNumber)
    {
        numberOfKidnapper = kidnapFinalNumber;
        introScreen.SetActive(true);
        isShowingIntro = true;
        introText.gameObject.SetActive(true);
        introTitle.gameObject.SetActive(true);
        introText.text = "There" + (kidnapFinalNumber < 2 ? "is " : "are ") + kidnapFinalNumber + " kidnapper" + (kidnapFinalNumber > 1 ? "s" : string.Empty);
        introTitle.text = "Welcome to the " + PhotonNetwork.CurrentRoom.Name + " village!";
        yield return new WaitForSeconds(2);
        introScreen.SetActive(false);
        introTitle.gameObject.SetActive(false);
        introText.gameObject.SetActive(false);
        isShowingIntro = false;
        gameplayInfo.SetActive(true);
    }
}
