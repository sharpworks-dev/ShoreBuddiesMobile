using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]float moveSpeed;
    [SerializeField]int inventorySize;
    [SerializeField]TextMeshProUGUI timerText;
    [SerializeField]TextMeshProUGUI itemsText;
    [SerializeField]TextMeshProUGUI scoreText;

    //Hide in inspect fields
    float timeRemaining = 10;
    bool timerIsRunning = false;
    SpriteRenderer renderer;
    Vector2 targetPos;
    int score = 0;
    int itemsCollected = 0;
    List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
    List<string> uniqueList = new List<string>();
    Dictionary<string, int> itemValues = new Dictionary<string, int>();

    int currentSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        itemValues.Add("flipflop", 5);
        itemValues.Add("can", 5);
        itemValues.Add("ring", 10); 
        itemValues.Add("battery", 20); 
        itemValues.Add("bottle", 5); 
        itemValues.Add("straw", 10); 
        itemValues.Add("toothbrush", 10); 
        itemValues.Add("milkcarton", 10);
        renderer = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.touchCount > 0){
            targetPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
        }

        if(targetPos.x != transform.position.x || targetPos.y != transform.position.y){
            if(targetPos.x > transform.position.x)
                renderer.flipX = true;
            else
                renderer.flipX = false;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed*Time.deltaTime);
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timerText.text = "Game over";
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }

        itemsText.text = "Items: " + currentSize + "/" + inventorySize;
        scoreText.text = "Score: " + score;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "item")
        {
            targetPos = new Vector2(transform.position.x, transform.position.y);
            if(currentSize < inventorySize)
            {
                int num = 0;
                
                AddPointsForItem(col.gameObject.name);
                currentSize++;
                Destroy(col.gameObject);
            }
            
        }

        else if (col.gameObject.tag == "Obstacle")
        {
            targetPos = new Vector2(transform.position.x, transform.position.y);
        }

        else if (col.gameObject.tag == "TrashBin" || col.gameObject.tag == "RecycleBin")
        {
            targetPos = new Vector2(transform.position.x, transform.position.y);
            foreach(KeyValuePair<string,int> item in list)
            {
                score += item.Value;
                itemsCollected ++;
            }
            list.Clear(); 
            currentSize = 0;
            Debug.Log(score);
            Debug.Log(itemsCollected);
        }
        
    }

    void AddPointsForItem(string itemName){
        if(uniqueList.Contains(itemName))
            score += itemValues[itemName];
        else{
            uniqueList.Add(itemName);
            score += (uniqueList.Count - 1) * 5;
        }
    }
}
