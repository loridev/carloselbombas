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

    public Button botonEmpezar;

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
}
