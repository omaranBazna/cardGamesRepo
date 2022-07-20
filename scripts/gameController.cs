using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class gameController : MonoBehaviourPunCallbacks
{
    public int myPlayer=0;

   public PhotonView view;
    List<GameObject> selectedCards = new List<GameObject>();
    public groundCards groundCardsObject;
    card card = new card();
    List<card> wholeCollection = new List<card>();
    public player player1, player2, player3, player4;
    player lastPlayer = new player();
    public GameObject cardObj;
    GameObject selected_card;
    int sum = 0;
    int current = 0;

    public Text scr1, scr2;
    int score1 = 0, score2 = 0;
    
    // Start is called before the first frame update

    public GameObject server;

    string set_select_card_name;
    
    void Start()
    {
        view = GetComponent<PhotonView>();
        player1 = GameObject.Find("player1").GetComponent<player>();
        player2 = GameObject.Find("player2").GetComponent<player>();
        player3 = GameObject.Find("player3").GetComponent<player>();
        player4 = GameObject.Find("player4").GetComponent<player>();
        groundCardsObject = GameObject.Find("ground_cards").GetComponent<groundCards>();
        scr1 = GameObject.Find("team1").GetComponent<Text>();
        scr2 = GameObject.Find("team2").GetComponent<Text>();
        server = GameObject.Find("Server");
        cardObj.GetComponent<card>().controller = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>().gC;
       

    }
    
    void gameStart()
    {


        card.type = "clubs";
            for (int i = 1; i < 11; i++)
            {
                card card2 = new card();
                card2.score = i;
                card2.link = card.type + "/" + score_to_link(i) + "_of_" + card.type;
                card2.type = card.type;
                wholeCollection.Add(card2);
            }
            card.type = "diamonds";
            for (int i = 1; i < 11; i++)
            {
                card card2 = new card();
                card2.score = i;
                card2.link = card.type + "/" + score_to_link(i) + "_of_" + card.type;
                card2.type = card.type;
                wholeCollection.Add(card2);
            }
            card.type = "hearts";
            for (int i = 1; i < 11; i++)
            {
                card card2 = new card();
                card2.score = i;
                card2.link = card.type + "/" + score_to_link(i) + "_of_" + card.type;
                card2.type = card.type;
                wholeCollection.Add(card2);
            }
            card.type = "spades";
            for (int i = 1; i < 11; i++)
            {
                card card2 = new card();
                card2.score = i;
                card2.link = card.type + "/" + score_to_link(i) + "_of_" + card.type;
                card2.type = card.type;
                wholeCollection.Add(card2);
            }


        


    }

 
    string score_to_link(int score)
    {
        switch (score)
        {
            case 1:
                return "ace";
                break;
            case 2:
                return "2";
                break;
            case 3:
                return "3";
                break;
            case 4:
                return "4";
                break;
            case 5:
                return "5";
                break;
            case 6:
                return "6";
                break;
            case 7:
                return "7";
                break;
            case 8:
                return "queen";
                break;
            case 9:
                return "jack";
                break;
            case 10:
                return "king";
                break;
        }
        return "";
    }


    void shuffle()
    {
        for (int i = 0; i < wholeCollection.Count; i++)
        {
            card temp = wholeCollection[i];
            int rnd = Random.Range(0, wholeCollection.Count);
            wholeCollection[i] = wholeCollection[rnd];
            wholeCollection[rnd] = temp;
        }
    }
    GameObject tempObj;
    void addCard_player(player ply)
    {
        shuffle();
        tempObj = Instantiate(cardObj);
        wholeCollection[wholeCollection.Count - 1].player = ply;
        wholeCollection[wholeCollection.Count - 1].setCard(tempObj);
        tempObj.transform.SetParent(ply.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(wholeCollection[wholeCollection.Count - 1].link);
        ply.addHand(tempObj);
      
        wholeCollection.RemoveAt(wholeCollection.Count - 1);

    }

    void addCard_ground()
    {
        shuffle();
        GameObject tempObj = Instantiate(cardObj);
        tempObj.GetComponent<card>().ground = true;
        wholeCollection[wholeCollection.Count - 1].setCard(tempObj);
        tempObj.transform.SetParent(groundCardsObject.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(wholeCollection[wholeCollection.Count - 1].link);
        groundCardsObject.addCard(tempObj);
        wholeCollection.RemoveAt(wholeCollection.Count - 1);

    }

    public int addSum(GameObject card)
    {
        if (!selectedCards.Contains(card))
        {

            sum += card.GetComponent<card>().score;

            selectedCards.Add(card);
            // view.RPC("add_selected_card", RpcTarget.All,card.gameObject.name);
            if (sum > 0 && sum == current)
            {

                kasha();
                sum = 0;
                ///view.RPC("renew_selected_card", RpcTarget.All);
                selectedCards = new List<GameObject>();
                return 1;
            }
        }
        return 0;
    }
    public void selectCard(GameObject card)
    {
        current = card.GetComponent<card>().score;

        selected_card = card;

        sum = 0;
        ///view.RPC("renew_selected_card", RpcTarget.All);
          selectedCards = new List<GameObject>();
    }
    public void put_on_ground()
    {
        if (selected_card)
        {
            
            view.RPC("putOnGroundf",RpcTarget.All, selected_card.GetComponent<card>().score, selected_card.GetComponent<card>().type, selected_card.GetComponent<card>().link,selected_card.name);
            view.RPC("arrangGroundf", RpcTarget.All);
            
            view.RPC("remove_from_hand", RpcTarget.All, selected_card.name);
            view.RPC("turn", RpcTarget.All);




            if (server.GetComponent<Server>().turn == "player1")
            {
                if (player1.transform.childCount == 0)
                {
                    /// newTurn();
               
                    view.RPC("setNextTurn", RpcTarget.All);
                }
            }
           
        }

    }
    public void kasha()
    {

      
       
        if (server.GetComponent<Server>().turn=="player1")
        {
        
            player1.GetComponent<player>().playerHand.Remove(selected_card.GetComponent<card>().type);
            view.RPC("remove_from_hand", RpcTarget.All, selected_card.name);
            view.RPC("turn", RpcTarget.All);

            view.RPC("AddCard_network1", RpcTarget.All, selected_card.GetComponent<card>().type);
            /// lastPlayer = player1;
            view.RPC("setLast", RpcTarget.All, 1);
            foreach (GameObject el in selectedCards)
            {
                Debug.LogError(el);
                view.RPC("AddCard_network1", RpcTarget.All, el.GetComponent<card>().type);
                if (selectedCards.Count == groundCardsObject.transform.childCount && wholeCollection.Count < 24)

                {
                    score1 += el.GetComponent<card>().score;

                }
               
                view.RPC("Destroy_from_ground", RpcTarget.All, el.name);

            }
          
            view.RPC("arrangGroundf", RpcTarget.All);
            view.RPC("arrang1", RpcTarget.All);






        }
        else if (server.GetComponent<Server>().turn == "player2")
        {


            player2.GetComponent<player>().playerHand.Remove(selected_card.GetComponent<card>().type);
            view.RPC("remove_from_hand", RpcTarget.All, selected_card.name);
            view.RPC("turn", RpcTarget.All);

            //  temp = Instantiate(selected_card);

            view.RPC("AddCard_network2", RpcTarget.All, selected_card.GetComponent<card>().type);
            view.RPC("setLast", RpcTarget.All, 2);
            foreach (GameObject el in selectedCards)
            {
                 view.RPC("AddCard_network2", RpcTarget.All, el.GetComponent<card>().type);

                if (selectedCards.Count == groundCardsObject.transform.childCount && wholeCollection.Count < 24)

                {

                    score1 += el.GetComponent<card>().score;


                }

                view.RPC("Destroy_from_ground", RpcTarget.All, el.name);



            }

            view.RPC("arrangGroundf", RpcTarget.All);
            view.RPC("arrang2", RpcTarget.All);



        }
        else if (server.GetComponent<Server>().turn == "player3")
        {
            

            player3.GetComponent<player>().playerHand.Remove(selected_card.GetComponent<card>().type);
            view.RPC("remove_from_hand", RpcTarget.All, selected_card.name);
            view.RPC("turn", RpcTarget.All);

            ///temp = Instantiate(selected_card);
            view.RPC("AddCard_network3", RpcTarget.All, selected_card.GetComponent<card>().type);
            view.RPC("remove_from_hand", RpcTarget.All, selected_card.name);
            view.RPC("setLast", RpcTarget.All, 2);
            foreach (GameObject el in selectedCards)
            {
                //temp = Instantiate(el);
                view.RPC("AddCard_network3", RpcTarget.All, el.GetComponent<card>().type);
                if (selectedCards.Count == groundCardsObject.transform.childCount && wholeCollection.Count < 24)

                {

                    score1 += el.GetComponent<card>().score;


                }

                view.RPC("Destroy_from_ground", RpcTarget.All, el.name);



            }

            view.RPC("arrangGroundf", RpcTarget.All);
            view.RPC("arrang3", RpcTarget.All);

        }
        else if (server.GetComponent<Server>().turn == "player4")
        {
            

            player4.GetComponent<player>().playerHand.Remove(selected_card.GetComponent<card>().type);
            view.RPC("remove_from_hand", RpcTarget.All, selected_card.name);
            view.RPC("turn", RpcTarget.All);

            ///  temp = Instantiate(selected_card);
            view.RPC("AddCard_network4", RpcTarget.All, selected_card.GetComponent<card>().type);


            view.RPC("setLast", RpcTarget.All, 4);
            foreach (GameObject el in selectedCards)
            {
                //temp = Instantiate(el);
                view.RPC("AddCard_network4", RpcTarget.All, el.GetComponent<card>().type);

                if (selectedCards.Count == groundCardsObject.transform.childCount && wholeCollection.Count < 24)

                {

                    score1 += el.GetComponent<card>().score;


                }

                view.RPC("Destroy_from_ground", RpcTarget.All, el.name);



            }

            view.RPC("arrangGroundf", RpcTarget.All);
            view.RPC("arrang4", RpcTarget.All);


            if (player1.transform.childCount == 0)
            {
                view.RPC("setNextTurn", RpcTarget.All);
                // newTurn();
            }


        }
    }
    public void newTurn()
    {



        if (wholeCollection.Count > 0)
        {
            Debug.LogError("whole collection :"+wholeCollection.Count.ToString());
                     CardsCollection_turn();


        }
        else
        {
            GameObject temp;
            GameObject el;
            for (int i = 0; i < groundCardsObject.gameObject.transform.childCount; i++)
            {

                el = groundCardsObject.gameObject.transform.GetChild(i).gameObject;
                Debug.Log(el.name);
               

                if (lastPlayer == player1)
                {
                    view.RPC("AddCard_network1", RpcTarget.All, el.GetComponent<card>().type);
                }else if(lastPlayer == player2)
                {
                    view.RPC("AddCard_network2", RpcTarget.All, el.GetComponent<card>().type);
                }
                else if (lastPlayer == player3)
                {
                    view.RPC("AddCard_network3", RpcTarget.All, el.GetComponent<card>().type);
                }
                else if (lastPlayer == player4)
                {
                    view.RPC("AddCard_network4", RpcTarget.All, el.GetComponent<card>().type);
                }
                view.RPC("Destroy_from_ground", RpcTarget.All, groundCardsObject.gameObject.transform.GetChild(i).gameObject.name);




            }
            bool heart = checkDiamonds();
            if (!heart)
            {
                checkSeven();
                if (player1.playerCards.Count + player3.playerCards.Count >= 21)
                {
                    score1 += 1;
                }
                if (player2.playerCards.Count + player4.playerCards.Count >= 21)
                {
                    score2 += 1;
                }

                checkMaxDiamonds();
                checkSevens();
               // scr1.GetComponent<Text>().text = "Team1:" + score1.ToString() + " Points";
                //scr2.GetComponent<Text>().text = "Team2:" + score2.ToString() + " Points";
                view.RPC("setScore", RpcTarget.All, score1,score2);
            }
            int upper = server.GetComponent<Server>().upperLimit;
            if (upper == 5)
            {
                if (score1 < 5 && score2 < 5)
                {


                    view.RPC("setGameStart", RpcTarget.All);

                }
                if (score1 >= 5 && score2 < 5)
                {
                    view.RPC("player1Win", RpcTarget.All);
                }
                if (score2 >= 5 && score1 < 5)
                {
                    view.RPC("player2Win", RpcTarget.All);
                }
                if (score2 >= 5 && score1 >= 5)
                {
                    view.RPC("newUpper", RpcTarget.All);
                }
            }
            if (upper == 7)
            {
                if (score1 < 7 && score2 < 7)
                {


                    view.RPC("setGameStart", RpcTarget.All);

                }
                if (score1 >= 7 && score2 < 7)
                {
                    view.RPC("player1Win", RpcTarget.All);
                }
                if (score2 >= 7 && score1 < 7)
                {
                    view.RPC("player2Win", RpcTarget.All);
                }
                if (score2 >= 7 && score1 >=7)
                {
                    if (score2 > score1)
                    {
                        view.RPC("player2Win", RpcTarget.All);
                    }else if(score2 < score1)
                    {
                        view.RPC("player1Win", RpcTarget.All);
                    }
                    else
                    {
                        view.RPC("bothWin", RpcTarget.All);
                    }
                    
                }
            }

        }

    }

    bool checkDiamonds()
    {

        int diamondCount = 0;
        foreach (string el in player1.playerCards)
        {

            if (el.Contains("diamonds"))
            {
                diamondCount++;
            }
        }
        foreach (string el in player3.playerCards)
        {
            if (el.Contains("diamonds"))
            {
                diamondCount++;
            }
        }

        if (diamondCount == 10)
        {
            score1 = 41;
            scr1.GetComponent<Text>().text = "Team1:41 points";
            return true;
        }

        if (diamondCount == 9)
        {
            score2 = 0;
            scr2.GetComponent<Text>().text = "Team2:0 points";
            return true;
        }
        if (diamondCount == 8)
        {
            score1 += 4;

            return false;
        }

        diamondCount = 0;
        foreach (string el in player2.playerCards)
        {
            if (el.Contains("diamonds"))
            {
                diamondCount++;
            }
        }
        foreach (string el in player4.playerCards)
        {
            if (el.Contains("diamonds"))
            {
                diamondCount++;
            }
        }
        if (diamondCount == 10)
        {
            score2 = 41;
            scr2.GetComponent<Text>().text = "Team2:41 points";
            return true;

        }
        if (diamondCount == 9)
        {

            score1 = 0;
            scr1.GetComponent<Text>().text = "Team1:0 points";
            return true;

        }
        if (diamondCount == 8)
        {
            score2 += 4;

            return false;
        }


        return false;

    }
    void checkSeven()
    {
        foreach (string el in player1.playerCards)
        {
            if (el.Contains( "diamonds" ) && type_score(el) == 7)
            {
                score1 += 1;
            }
        }
        foreach (string el in player3.playerCards)
        {
            if (el.Contains("diamonds") && type_score(el) == 7)
            {
                score1 += 1;
            }
        }



        foreach (string el in player2.playerCards)
        {
            if (el.Contains("diamonds") && type_score(el) == 7)
            {
                score2 += 1;
            }
        }
        foreach (string el in player4.playerCards)
        {
            if (el.Contains("diamonds") && type_score(el) == 7)
            {
                score2 += 1;
            }
        }




    }
    void checkMaxDiamonds()
    {
        int team1 = 0, team2 = 0;
        foreach (string el in player1.playerCards)
        {
            if (el.Contains("diamonds"))
            {
                team1++;
            }
        }
        foreach (string el in player3.playerCards)
        {
            if (el.Contains("diamonds"))
            {
                team1++;
            }
        }



        foreach (string el in player2.playerCards)
        {
            if (el.Contains("diamonds"))
            {
                team2++;
            }
        }
        foreach (string el in player4.playerCards)
        {
            if (el.Contains("diamonds"))
            {
                team2++;
            }
        }
        if (team1 > team2)
        {
            score1++;

        }
        if (team2 > team1)
        {
            score2++;
        }
    }
    void checkSevens()
    {
        int team1 = 0, team2 = 0;
        foreach (string el in player1.playerCards)
        {
            if (type_score(el) == 7)
            {
                team1++;
            }
        }
        foreach (string  el in player3.playerCards)
        {
            if (type_score(el) == 7)
            {
                team1++;
            }
        }
        if (team1 >= 3)
        {

            score1++;



        }
        else if (team1 == 2)
        {

            int temp = 0;
            foreach (string el in player1.playerCards)
            {
                if (type_score(el) == 6)
                {
                    temp++;
                }
            }
            foreach (string el in player3.playerCards)
            {
                if (type_score(el) == 6)
                {
                    temp++;
                }
            }

            if (temp >= 3)
            {
                score1++;

            }


        }
        if (team1 == 4) { score1 += 6; }

        foreach (string el in player2.playerCards)
        {
            if (type_score(el) == 7)
            {
                team2++;
            }
        }
        foreach (string el in player4.playerCards)
        {
            if (type_score(el) == 7)
            {
                team2++;
            }
        }
        if (team2 >= 3)
        {

            score2++;



        }
        else if (team2 == 2)
        {

            int temp = 0;
            foreach (string el in player2.playerCards)
            {
                if (type_score(el) == 6)
                {
                    temp++;
                }
            }
            foreach (string el in player4.playerCards)
            {
                if (type_score(el) == 6)
                {
                    temp++;
                }
            }

            if (temp >= 3)
            {
                score2++;

            }


        }
        if (team2 == 4) { score2 += 6; }




    }

    public int type_score(string type)
    {

        if (type.Contains("2")) { return 2; }
        if (type.Contains("3")) { return 3; }
        if (type.Contains("4")) { return 4; }
        if (type.Contains("5")) { return 5; }
        if (type.Contains("6")) { return 6; }
        if (type.Contains("7")) { return 7; }
        if (type.Contains("ace")) { return 1; }
        if (type.Contains("queen")) { return 8; }
        if (type.Contains("jack")) { return 9; }
        if (type.Contains("king")) { return 10; }
        return 0;

    }
    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.A) && myPlayer == 0)
            {
                view.RPC("selectPlayer", RpcTarget.All, 0);
            }
            if (Input.GetKeyDown(KeyCode.S) && myPlayer == 0)
            {
                view.RPC("selectPlayer", RpcTarget.All, 1);
            }
            if (Input.GetKeyDown(KeyCode.D) && server.GetComponent<Server>().fill==2 && myPlayer==1)
            {
                gameStart();
                CardsCollection();
            }
            if(Input.GetKeyDown(KeyCode.F) && myPlayer==1  && server.GetComponent<Server>().newTurn_time)
            {
     
                server.GetComponent<Server>().newTurn_time = false;
                newTurn();
            }
            if (Input.GetKeyDown(KeyCode.F) && myPlayer == 1 && server.GetComponent<Server>().gameStart)
            {
                
                server.GetComponent<Server>().gameStart= false;
                gameStart();
                CardsCollection();
            }
            if (Input.GetKeyDown(KeyCode.Q) )
            {
                Debug.Log("put on the ground");
                put_on_ground();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                for(int i = 0; i < wholeCollection.Count; i++)
                {
                    Debug.Log(wholeCollection[i].link);
                }
            }
        }
    }
    public void CardsCollection()
    {



        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player1;
        int score = wholeCollection[wholeCollection.Count - 1].score;
        string type = wholeCollection[wholeCollection.Count - 1].type;
        string link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard1", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player1;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard1", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player1;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard1", RpcTarget.All, score, type, link);
        view.RPC("arrang1", RpcTarget.All);




        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player2;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard2", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player2;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard2", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player2;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard2", RpcTarget.All, score, type, link);

        view.RPC("arrang2", RpcTarget.All);



        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player3;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard3", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player3;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard3", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player3;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard3", RpcTarget.All, score, type, link);

        view.RPC("arrang3", RpcTarget.All);




        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard4", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard4", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard4", RpcTarget.All, score, type, link);
        view.RPC("arrang4", RpcTarget.All);



        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerGround", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerGround", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerGround", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerGround", RpcTarget.All, score, type, link);

        view.RPC("arrangGroundf", RpcTarget.All);
       



    }
    public void CardsCollection_turn()
    {
        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player1;
        int score = wholeCollection[wholeCollection.Count - 1].score;
        string type = wholeCollection[wholeCollection.Count - 1].type;
        string link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard1", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player1;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard1", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player1;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard1", RpcTarget.All, score, type, link);
        view.RPC("arrang1", RpcTarget.All);




        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player2;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard2", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player2;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard2", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player2;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard2", RpcTarget.All, score, type, link);

        view.RPC("arrang2", RpcTarget.All);



        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player3;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard3", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player3;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard3", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player3;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard3", RpcTarget.All, score, type, link);

        view.RPC("arrang3", RpcTarget.All);




        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard4", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard4", RpcTarget.All, score, type, link);

        shuffle();
        wholeCollection[wholeCollection.Count - 1].player = player4;
        score = wholeCollection[wholeCollection.Count - 1].score;
        type = wholeCollection[wholeCollection.Count - 1].type;
        link = wholeCollection[wholeCollection.Count - 1].link;
        wholeCollection.RemoveAt(wholeCollection.Count - 1);
        view.RPC("playerCard4", RpcTarget.All, score, type, link);
        view.RPC("arrang4", RpcTarget.All);



    }


    [PunRPC]
    public void selectPlayer(int selected)
    {
        if(selected == 0)
        {
            GameObject.Find("choose1").gameObject.SetActive(false);
            myPlayer = 1;
            server.GetComponent<Server>().fill++;

        }else if(selected == 1)
        {
            GameObject.Find("choose2").gameObject.SetActive(false);
            server.GetComponent<Server>().fill++;
            myPlayer = 2;
        
        }
    }
    [PunRPC]
    public void playerCard1(int score,string type,  string link )
    {
        /*
        addObj.transform.SetParent(player1.transform);
        
        //player1.addHand(tempObj);

        player1.arrangPlayer(-250, -12, -50, -12, 0);
        */
        tempObj = Instantiate(cardObj);
        tempObj.transform.SetParent(player1.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(link);
        tempObj.GetComponent<card>().score = score;
        tempObj.GetComponent<card>().type = type;
        tempObj.GetComponent<card>().player =player1;
        tempObj.GetComponent<card>().link = link;
        tempObj.GetComponent<card>().controller = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>().gC;
        tempObj.name = tempObj.GetComponent<card>().type = type + "_" + tempObj.GetComponent<card>().score.ToString();
    }

    [PunRPC]
    public void playerCard2(int score, string type, string link)
    {
        /*
        addObj.transform.SetParent(player1.transform);
        
        //player1.addHand(tempObj);

        player1.arrangPlayer(-250, -12, -50, -12, 0);
        */
        tempObj = Instantiate(cardObj);
        tempObj.transform.SetParent(player2.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(link);
        tempObj.GetComponent<card>().score = score;
        tempObj.GetComponent<card>().type = type;
        tempObj.GetComponent<card>().player = player2;
        tempObj.GetComponent<card>().link = link;
        tempObj.GetComponent<card>().controller = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>().gC;
        tempObj.name = tempObj.GetComponent<card>().type = type + "_" + tempObj.GetComponent<card>().score.ToString();

    }
    [PunRPC]
    public void playerCard3(int score, string type, string link)
    {
        /*
        addObj.transform.SetParent(player1.transform);
        
        //player1.addHand(tempObj);

        player1.arrangPlayer(-250, -12, -50, -12, 0);
        */
        tempObj = Instantiate(cardObj);
        tempObj.transform.SetParent(player3.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(link);
        tempObj.GetComponent<card>().score = score;
        tempObj.GetComponent<card>().type = type;
        tempObj.GetComponent<card>().player = player3;
        tempObj.GetComponent<card>().link = link;
        tempObj.GetComponent<card>().controller = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>().gC;;
        tempObj.name = tempObj.GetComponent<card>().type = type + "_" + tempObj.GetComponent<card>().score.ToString();
    }

    [PunRPC]
    public void playerCard4(int score, string type, string link)
    {
        /*
        addObj.transform.SetParent(player1.transform);
        
        //player1.addHand(tempObj);

        player1.arrangPlayer(-250, -12, -50, -12, 0);
        */
        tempObj = Instantiate(cardObj);
        tempObj.transform.SetParent(player4.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(link);
        tempObj.GetComponent<card>().score = score;
        tempObj.GetComponent<card>().type = type;
        tempObj.GetComponent<card>().player = player4;
        tempObj.GetComponent<card>().link = link;
        tempObj.GetComponent<card>().controller = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>().gC;;
        tempObj.name = tempObj.GetComponent<card>().type = type + "_" + tempObj.GetComponent<card>().score.ToString();

    }
    [PunRPC]
    public void playerGround(int score, string type, string link)
    {
        /*
        addObj.transform.SetParent(player1.transform);
        
        //player1.addHand(tempObj);

        player1.arrangPlayer(-250, -12, -50, -12, 0);
        */
        tempObj = Instantiate(cardObj);
        tempObj.transform.SetParent(groundCardsObject.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(link);
        tempObj.GetComponent<card>().score = score;
        tempObj.GetComponent<card>().type = type;
        tempObj.GetComponent<card>().player = null;
        tempObj.GetComponent<card>().link = link;
        tempObj.GetComponent<card>().ground = true;
        tempObj.name = tempObj.GetComponent<card>().type = type + "_" + tempObj.GetComponent<card>().score.ToString();

    }
    [PunRPC]
    public void arrang1()
    {
        player1.arrangPlayer(-250, -12, -50, -12, 0);

    }
    [PunRPC]
    public void arrang2()
    {
        player2.arrangPlayer(11, -50, 11, -258, 90);
    }
    [PunRPC]
    public void arrang3()
    {
        player3.arrangPlayer(50, 9.6f, 250, 9.6f, 180);
    }
    [PunRPC]
    public void arrang4()
    {
        player4.arrangPlayer(-7.7f, 208, -7.7f, 0, -90);
    }
    [PunRPC]
    public void arrangGroundf()
    {
        groundCardsObject.arrangGround();
    }

    [PunRPC]
    public void putOnGroundf(int score, string type, string link,string name)
    {
        tempObj = Instantiate(cardObj);
        tempObj.transform.SetParent(groundCardsObject.transform);
        tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(link);
        tempObj.GetComponent<card>().score = score;
        tempObj.GetComponent<card>().type = type;
        tempObj.GetComponent<card>().player = null;
        tempObj.GetComponent<card>().link = link;
        tempObj.GetComponent<card>().ground = true;
        tempObj.name = name;
    }
    [PunRPC]
    public void turn()
    {
        if (server.GetComponent<Server>().turn == "player1")
        {
            server.GetComponent<Server>().turn = "player2";
        }else if (server.GetComponent<Server>().turn == "player2")
        {
            server.GetComponent<Server>().turn = "player3";
        }
        else if (server.GetComponent<Server>().turn == "player3")
        {
            server.GetComponent<Server>().turn = "player4";
        }
        else if (server.GetComponent<Server>().turn == "player4")
        {
            server.GetComponent<Server>().turn = "player1";
        }

    }
    [PunRPC]
    public void remove_from_hand(string OBname)
    {
        if (server.GetComponent<Server>().turn == "player1")
        {
            Destroy(GameObject.Find( OBname));
      
        }
        else if (server.GetComponent<Server>().turn == "player2")
        {
            Destroy(GameObject.Find(OBname));
       
        }
        else if (server.GetComponent<Server>().turn == "player3")
        {
            Destroy(GameObject.Find(OBname));
         
        }
        else if (server.GetComponent<Server>().turn == "player4")
        {
            Destroy(GameObject.Find(OBname));
           
        }

    }

    [PunRPC]
    public void Destroy_from_ground(string OBname)
    {
        Destroy(GameObject.Find(OBname));

    }
    [PunRPC]
    public void renew_selected_card()
    {
        selectedCards = new List<GameObject>();
    }
    [PunRPC]
    public void add_selected_card(string card_name)
    {
        selectedCards.Add(GameObject.Find(card_name));
    }
    [PunRPC]
    public void set_selected_card(string name)
    {
        set_select_card_name=name;
    }
    [PunRPC]
    public void setNextTurn()
    {
        server.GetComponent<Server>().newTurn_time = true;
    }
    [PunRPC]
    public void setGameStart()
    {
        server.GetComponent<Server>().gameStart = true;
    }
    [PunRPC]
    public void AddCard_network1(string type)
    {
        player1.addCard(type);
    }
    [PunRPC]
    public void AddCard_network2(string type)
    {
        player2.addCard(type);
    }
    [PunRPC]
    public void AddCard_network3(string type)
    {
        player3.addCard(type);
    }
    [PunRPC]
    public void AddCard_network4(string type)
    {
        player4.addCard(type);
    }
    [PunRPC]
    public void setLast(int num)
    {
        if (num == 1)
        {
            lastPlayer = player1;
        }
        if (num == 2)
        {
            lastPlayer = player2;
        }
        if (num == 3)
        {
            lastPlayer = player3;
        }
        if (num == 4)
        {
            lastPlayer = player4;
        }

    }

    [PunRPC]

    public void setScore(int score1,int score2)
    {
        scr1.GetComponent<Text>().text = "Team1:" + score1.ToString() + " Points";
        scr2.GetComponent<Text>().text = "Team2:" + score2.ToString() + " Points";
    }
    [PunRPC]
    public void player1Win()
    {
        Debug.LogError("player1Win");
    }
    [PunRPC]
    public void player2Win()
    {
        Debug.LogError("player1Win");
    }
    [PunRPC]
    public void bothWin()
    {
        Debug.LogError("player1Win");
    }
    [PunRPC]
    public void newUpper()
    {
        server.GetComponent<Server>().upperLimit = 7;
    }
}

