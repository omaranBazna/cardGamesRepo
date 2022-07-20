using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class ConnectToServer :MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.LogError("start");
    }

    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinLobby();
        Debug.LogError("connect");
    }
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene(1);
    }
}
