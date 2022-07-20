using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class move : MonoBehaviour
{
    // Start is called before the first frame update
    PhotonView view;


    void Start()
    {
          view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1f, 0, 0);
                view.RPC("changeText", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void changeText()
    {
        GameObject.Find("player1").transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.blue;
    }
}
