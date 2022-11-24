using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class HomeCharacterSelection : MonoBehaviour
{
    public Image playerAvatar;
    public Sprite[] avatars;

    public int currentPlayerAvatarIndex;

    public bool isHomeCharacterSkinSelected = false;

    public void GetAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    void OnDataReceived(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("playerSkin"))
        {
            currentPlayerAvatarIndex = int.Parse(result.Data["playerSkin"].Value);
            UpdateAppearance();
        }
        else
        {
            Debug.Log("Player Appearance Data Could Not Be Found!");
        }
    }


    public void OnClickSaveAppearance()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"playerSkin", currentPlayerAvatarIndex.ToString()},
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Player Appearance Selected! Player Skin: " + currentPlayerAvatarIndex);
        isHomeCharacterSkinSelected = true;
    }

    void OnError(PlayFabError error)
    {

    }


    public void OnClickLeftArrow()
    {
        if (currentPlayerAvatarIndex == 0)
        {
            currentPlayerAvatarIndex = avatars.Length - 1;
        }
        else
        {
            currentPlayerAvatarIndex = currentPlayerAvatarIndex - 1;
        }
        UpdateAppearance();
    }

    public void OnClickRightArrow()
    {
        if (currentPlayerAvatarIndex == avatars.Length - 1)
        {
            currentPlayerAvatarIndex = 0;
        }
        else
        {
            currentPlayerAvatarIndex = currentPlayerAvatarIndex + 1;
        }
        UpdateAppearance();
    }

    void UpdateAppearance()
    {
       playerAvatar.sprite = avatars[currentPlayerAvatarIndex];
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
