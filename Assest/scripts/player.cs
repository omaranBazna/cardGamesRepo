using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class player : MonoBehaviour
{
    private void Start()
    {
    
    }


    public List<string> playerHand=new List<string>();
    public List<string> playerCards = new List<string>();

    GameObject[] groundCards;
    GameObject select = null;
    public bool turn = false;
   
    // Start is called before the first frame update
    /*
    public void clearPlayer()
    {
        select = null;
        turn = false;
        foreach (card el in playerHand)
        {
            el.GetComponent<Image>().color = Color.white;
        }
    }
    */
    
    public void selectCard(GameObject card)
    {
        
      
            for(int i=0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
           
            select = card;
            card.GetComponent<Image>().color = Color.blue;
        
        

    }

    public void addHand(GameObject card)
    {
 
        playerHand.Add(card.GetComponent<card>().type);
      
    }
    public void addCard(string type)
    {

        playerCards.Add(type);

    }


    public void removeHand(GameObject card)
    {
        playerHand.Remove(card.GetComponent<card>().type);
       for (int i = 0; i < gameObject.transform.childCount; i++)
        {
      
            if(gameObject.transform.GetChild(i).gameObject.GetComponent<card>() == card.GetComponent<card>())
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);   
            }
        }
      
    }
    public void removeHand2(string cardName)
    {
        GameObject card = GameObject.Find(cardName);
      ///  playerHand.Remove(card);
      
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
           
           
            if (gameObject.transform.GetChild(i).gameObject.GetComponent<card>().type == card.GetComponent<card>().type )
            {

                if (gameObject.transform.GetChild(i).gameObject.GetComponent<card>().score== card.GetComponent<card>().score)
                {
                    Destroy(gameObject.transform.GetChild(i).gameObject);
                }
            }
            
        }

    }


    public void arrangPlayer(float startX,float startY,float endX,float endY,float angle)
    {
        int count = gameObject.transform.childCount;
       
        float deltaX=0,deltaY=0;
        if (count > 0)
        {
            deltaX = (startX - endX) / count;
            deltaY = (startY - endY) / count;
        }
        for (int i = 0; i < count; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<RectTransform>().localPosition= new Vector2(startX - i * deltaX, startY - i * deltaY);
            gameObject.transform.GetChild(i).GetComponent<RectTransform>().RotateAroundLocal(new Vector3(0, 0, 1), Mathf.Deg2Rad*angle);
        }
    }
}
