using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]int playerSelection;
    [SerializeField]float moveSpeed;
    [SerializeField]int inventorySize;

    //Hide in inspect fields
    int[] characterMoveSpeeds = new int[5]{60, 40, 20, 15, 10};
    int[] characterInventorySizes = new int[5]{1, 2, 4, 6, 8};
    SpriteRenderer renderer;
    Vector2 targetPos;
    int itemsCollected = 0;
    int scoreTemp = 0;
    List<string> uniqueList = new List<string>();
    Dictionary<string, int> itemValues = new Dictionary<string, int>();

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
        moveSpeed = characterMoveSpeeds[childIndex];
        inventorySize = characterInventorySizes[childIndex];
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
