using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DayNightCycle : MonoBehaviourPunCallbacks, IPunObservable
{

    PhotonView newView;

    public GameObject globalVolume;

    public Volume volume;

    public float tick;
    public static float seconds;
    public static int mins;
    public static int hours;
    public int days = 1;
    public Text timeDisplay;
    public Text dayDisplay;
    public static bool activateLights;
    public GameObject[] lights;


    void Start()
    {
        volume = globalVolume.GetComponent<Volume>();
        newView = GetComponent<PhotonView>();
        hours = 7;
    }

    void FixedUpdate()
    {
        CalculateTime();
        DisplayTime();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            newView.RPC("RPC_SendTimer", RpcTarget.Others, hours);
        }
    }

    [PunRPC]
    private void RPC_SendTimer(int timeIn)
    {
        //RPC for syncing the countdown Timer to those that join after it has started the countdown
        hours = timeIn;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

    }

    public void CalculateTime()
    {
        tick = 0.000001f; 
        seconds += (float)PhotonNetwork.Time * tick;

        if (seconds >= 60)
        {
            seconds = 0;
            mins += 1;
        }
        
        if(mins >= 60)
        {
            mins = 0;
            hours += 1;
        }

        if(hours >= 24)
        {
            hours = 0;
            days += 1;
        }

        ControlVolume();
    }

    public void ControlVolume()
    {
        if (hours >= 16 && hours < 17)
        {
            volume.weight = (float)mins / 60;
            if (!activateLights)
            {
                if(mins > 20)
                {
                    activateLights = true;
                }
            }
        }

        if(hours >= 6 && hours < 7)
        {
            volume.weight = 1 - (float)mins / 60;
            if (activateLights)
            {
                if(mins > 40)
                {
                    activateLights = false;
                }
            }
        }
    }

    public void DisplayTime()
    {
        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins);
        dayDisplay.text = "Day: " + days;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hours);
            stream.SendNext(mins);
            stream.SendNext(seconds);
            stream.SendNext(days);
        }
        else if (stream.IsReading)
        {
            hours = (int)stream.ReceiveNext();
            mins = (int)stream.ReceiveNext();
            seconds = (float)stream.ReceiveNext();
            days = (int)stream.ReceiveNext();
        }
    }
}
