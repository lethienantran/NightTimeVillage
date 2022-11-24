using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayFabAuth : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loginTab, signUpTab;
    public Text usernameSignUp, userpasswordSignUp, userPlayerNameSignUp, userEmailSignUp, usernameLogin, userPasswordLogin, signUpError, loginError, loginSuccess;

    public string _playFabPlayerIdCache;
    public void GetAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Player Appearance Data Found!");
        if (result.Data != null && result.Data.ContainsKey("playerSkin"))
        {
            SceneManager.LoadScene("Home");
        }
        else
        {
            SceneManager.LoadScene("CreateCharacter");
            Debug.Log("Player Appearance Data Could Not Be Found!");
        }
    }

    void OnError(PlayFabError error)
    {

    }

    public void SignUp()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Password = userpasswordSignUp.text, Username = usernameSignUp.text, DisplayName = userPlayerNameSignUp.text, Email = userEmailSignUp.text };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterError);

    }

    public void LogInWithPlayFab()
    {
        var request = new LoginWithPlayFabRequest();
        request.Username = usernameLogin.text;
        request.Password = userPasswordLogin.text;
        request.InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true };
        PlayFabClientAPI.LoginWithPlayFab(request, RequestPhotonToken, LoginError);
    }

    private void LoginError(PlayFabError error)
    {
        loginError.text = error.GenerateErrorReport();
    }
    private void RegisterError (PlayFabError error)
    {
        signUpError.text = error.GenerateErrorReport();
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        signUpError.text = "";
        LoginTab();
    }

    public void LoginSuccess()
    {
        loginError.text = "";
        signUpError.text = "";
        loginSuccess.text = "Connecting...";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player Name: " + PhotonNetwork.NickName);
        GetAppearance();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignUpTab()
    {
        signUpTab.SetActive(true);
        loginTab.SetActive(false);
        loginError.text = "";
        signUpError.text = "";
        loginSuccess.text = "";
    }

    public void LoginTab()
    {
        signUpTab.SetActive(false);
        loginTab.SetActive(true);
        loginError.text = "";
        signUpError.text = "";
        loginSuccess.text = "";
    }

    private void RequestPhotonToken(LoginResult obj)
    {
        LoginSuccess();
        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
        }, LoginWithPhoton, LoginError);
    }

    private void LoginWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        LoginSuccess();

        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);

        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);
        PhotonNetwork.AuthValues = customAuth;
    }
}
