using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class CharacterCreator : MonoBehaviour
{
    public GameObject LeftArrowButton;
    public GameObject RightArrowButton;

    public Image playerAvatar;
    public Sprite[] avatars;

    public int currentPlayerAvatarIndex;

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
        SceneManager.LoadScene("Home");
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Player Appearance Saved! Player Skin: " + currentPlayerAvatarIndex);
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
        currentPlayerAvatarIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
