using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefap;
    GameObject temp;
   public gameController gC;
    private void Start()
    {
        
        temp=PhotonNetwork.Instantiate(playerPrefap.name, new Vector3(-4.42f, 0, 0),Quaternion.identity);
        gC=temp.GetComponent<gameController>();
    }
}
