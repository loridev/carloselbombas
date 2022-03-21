using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    [SerializeField] public string VersioName = "0.1";
    [SerializeField] public GameObject UsernameMenu;
    [SerializeField] public GameObject ConnectPanel;

    [SerializeField] public InputField UsernameInput;
    [SerializeField] public InputField CreateGameInput;
    [SerializeField] public InputField JoinGameInput;

    public void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VersioName);
    }

    public void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }
    // Start is called before the first frame update
    public void Start()
    {
        UsernameMenu.setActive(true);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
