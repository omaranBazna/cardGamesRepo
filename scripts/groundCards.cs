using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCards : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> ground_Cards=new List<GameObject>();
    private void Start()
    {
        arrangGround();
    }
    public void addCard(GameObject obj)
    {
        ground_Cards.Add(obj);
    }
    public void arrangGround()
    {
        int count = gameObject.transform.childCount;
        int row = count / 5+1;
        int width = 5;

        int index = 0;
        if (row < 3)
        {
            for (int i = 0; i < row; i++)
            {
                for (int u = 0; u < 5; u++)
                {
                    if (index < count)
                    {
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().localPosition = new Vector2(-100f + u * 60f, -75f + i * 100f);
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
                        index++;
                    }
                }
            }
        }
        else if(row>2 && row <4)
        {

            for (int i = 0; i < row; i++)
            {
                for (int u = 0; u < 5; u++)
                {
                    if (index < count)
                    {
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().sizeDelta = new Vector2(57f, 82.7f)*0.75f;
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().localPosition = new Vector2(-100f + u * 60f, -75f + i * 75f);
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
                        index++;
                    }
                }
            }

        }
        else
        {
            

            for (int i = 0; i < row; i++)
            {
                for (int u = 0; u < 8; u++)
                {
                    if (index < count)
                    {
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().sizeDelta = new Vector2(57f, 82.7f) * 0.65f;
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().localPosition = new Vector2(-100f + u * 35f, -90f + i * 46f);
                        gameObject.transform.GetChild(index).GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
                        index++;
                    }
                }
            }



        }

    }
}
