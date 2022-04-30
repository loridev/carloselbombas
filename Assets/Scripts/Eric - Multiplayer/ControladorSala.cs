using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorSala : MonoBehaviour
{
    private string version = "0.1";
    public InputField salaCrear;
    public InputField salaUnir;

    public Button botonCrear;
    public Button botonUnir;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(version);
    }

    private void Start()
    {
        PhotonNetwork.playerName = Globals.CurrentUser.name;
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Conectado");
    }

    public void CrearSala()
    {
        PhotonNetwork.CreateRoom(salaCrear.text, new RoomOptions() {MaxPlayers = 4}, null);
    }

    public void UnirseSala()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(salaUnir.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MapaDinamicoFinal");
    }
}
