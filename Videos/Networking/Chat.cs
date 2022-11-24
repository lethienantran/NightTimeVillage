using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Chat : MonoBehaviour
{

    public InputField ChatInput;
    public TextMeshProUGUI ChatContent;

    private PhotonView photonView;
    private List<string> _messages = new List<string>();
    private float _buildDelay = 0f;
    public int _maximumMessages;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    void RPC_AddNewMessage(string msg)
    {
        _messages.Add(msg);
    }

    public void SendChat(string msg)
    {
        string NewMessage = PhotonNetwork.NickName + ": " + msg;
        photonView.RPC("RPC_AddNewMessage", RpcTarget.All, NewMessage);
    }

    public void SubmitChat()
    {
        string blankCheck = ChatInput.text;
        blankCheck = Regex.Replace(blankCheck, @"\s", "");
        if (blankCheck == "")
        {
            ChatInput.ActivateInputField();
            ChatInput.text = "";
            return;
        }

        SendChat(ChatInput.text);
        ChatInput.ActivateInputField();
        ChatInput.text = "";

    }

    void BuildChatContents()
    {
        string NewContents ="";
        foreach(string s in _messages)
        {
            NewContents += s + "\n";
        }
        ChatContent.text = NewContents;
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            ChatContent.maxVisibleLines = _maximumMessages;
            if(_messages.Count > _maximumMessages)
            {
                _messages.RemoveAt(0);
            }
            if(_buildDelay < Time.time)
            {
                BuildChatContents();
                _buildDelay = Time.time + 0.25f;
            }
        }
        else if(_messages.Count > 0)
        {
            _messages.Clear();
            ChatContent.text = "";
        }
    }
}
