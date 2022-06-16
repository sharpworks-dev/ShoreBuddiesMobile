using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]int playerSelection;
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
    int scoreTemp = 0;
    int itemsCollected = 0;
    List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
    List<string> uniqueList = new List<string>();
    Dictionary<string, int> itemValues = new Dictionary<string, int>();
    string[] items;

    int currentSize = 0;

    // Start is called before the first frame update.
    void Start()
    {
        playerSelection = 1;
        EnableSelectedCharacter(playerSelection);
        itemValues.Add("flipflop", 5);
        itemValues.Add("can", 5);
        itemValues.Add("bottle", 5); 
        itemValues.Add("ring", 10); 
        itemValues.Add("straw", 10); 
        itemValues.Add("toothbrush", 10); 
        itemValues.Add("milkCarton", 10);
        itemValues.Add("battery", 20); 
        targetPos = transform.position;
        timerIsRunning = true;
        items = new string[inventorySize];
    }

    // Update is called once per frame.
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

    void EnableSelectedCharacter(int childIndex){
        for(int i = 0; i < transform.childCount; i++){
            this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            if(i == childIndex){
                GameObject player = this.gameObject.transform.GetChild(i).gameObject;
                player.SetActive(true);
                renderer = player.GetComponent<SpriteRenderer>();
            }
        }
        switch(childIndex){
            case 0:
                moveSpeed = 60;
                inventorySize = 1;
                break;
            case 1:
                moveSpeed = 40;
                inventorySize = 2;
                break;
            case 2: 
                moveSpeed = 20;
                inventorySize = 4;
                break;
            case 3:
                moveSpeed = 15;
                inventorySize = 6;
                break;
            case 4:
                moveSpeed = 10;
                inventorySize = 8;
                break;
        }
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
            score += scoreTemp;
            scoreTemp = 0;
            currentSize = 0;
            // Debug.Log(score);
            // Debug.Log(itemsCollected);
        }
        
    }

    void AddPointsForItem(string itemName){
        scoreTemp += itemValues[itemName];
        if(!uniqueList.Contains(itemName)){
            scoreTemp += (uniqueList.Count - 1) * 5;
            uniqueList.Add(itemName);
        }
        Debug.Log("scoreTemp: " + scoreTemp);
    }
}
