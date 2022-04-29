using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public string VersionName = "0.1";
    public int playersCount = 0;
    public GameObject UsernameMenu;
    public GameObject ConnectPanel;

    public InputField UsernameInput;
    public InputField CreateGameInput;
    public InputField JoinGameInput;

    public GameObject StartButton;

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VersionName);
    }

    // Start is called before the first frame update
    void Start()
    {
        UsernameMenu.SetActive(true);
    }

    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void ChangeUserNameInput()
    {
        if(UsernameInput.text.Length >= 3)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void setUsername()
    {
        UsernameMenu.SetActive(false);
        PhotonNetwork.playerName = UsernameInput.text;
    }

    public void CreateGame()
    {
        playersCount += 1;
        Debug.Log(playersCount);
        Globals.WorldNum = 4;
        Globals.LevelNum = 5;
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { maxPlayers = 4 }, null);
    }

    public void JoinGame()
    {
        playersCount += 1;
        if (JoinGameInput.text.Length == 0)
        { 
            RoomOptions roomoptions = new RoomOptions();
            roomoptions.maxPlayers = 4;
            PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomoptions, TypedLobby.Default);
        }
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGame");
    }

    public void OnOnlineRoom()
    {
        PhotonNetwork.LoadLevel("MainMenu");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
