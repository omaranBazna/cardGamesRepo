using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update
    public int score;
    public string type;
    public player player;
    public string link = "";
    public bool ground = false;
    public gameController controller;
 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
          
            if (player )
            {
                string turn =controller.server.GetComponent<Server>().turn;
                int myPlayer = controller.myPlayer;
                Debug.LogError(player.name);
                Debug.LogError(myPlayer);
                if (turn ==player.name)
                {
                    if (myPlayer == 1 && (turn == "player1" || turn == "player3"))
                    {
                        player.selectCard(this.gameObject);
                        controller.selectCard(this.gameObject);
                    }

                    if (myPlayer == 2 && (turn == "player2" || turn == "player4"))
                    {
                        player.selectCard(this.gameObject);
                        controller.selectCard(this.gameObject);
                    }
                }

            }
            if (ground)
            {
                controller.addSum(this.gameObject);
               
                //controller.view.RCP("set_selected_card", RpcTarget.All,this.gameObject.name);
            }
        }
    }
    public void setCard(GameObject temp)
    {
        temp.GetComponent<card>().score = score;
        temp.GetComponent<card>().type = type;

        temp.GetComponent<card>().player = player;
        temp.GetComponent<card>().link = link;

    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ground)
        {
            gameObject.transform.localPosition += gameObject.transform.up * 15f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ground)
        {
            gameObject.transform.localPosition -= gameObject.transform.up * 15f;
        }
    }

 
}
