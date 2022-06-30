using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]float moveSpeed;
    [SerializeField]int inventorySize;
    [SerializeField]TextMeshProUGUI timerText;
    [SerializeField]TextMeshProUGUI itemsText;
    [SerializeField]TextMeshProUGUI scoreText;
    [SerializeField]Camera myCamera;
    [SerializeField]Canvas myCanvas;
    [SerializeField]CinemachineVirtualCamera vCam;

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
    PhotonView view;

    int currentSize = 0;
    bool CR_running = false;

    // Start is called before the first frame update.
    void Start()
    {
        itemValues.Add("flipflop", 5);
        itemValues.Add("can", 5);
        itemValues.Add("bottle", 5); 
        itemValues.Add("ring", 10); 
        itemValues.Add("straw", 10); 
        itemValues.Add("toothbrush", 10); 
        itemValues.Add("milkCarton", 10);
        itemValues.Add("battery", 20); 
        renderer = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
        timerIsRunning = true;
        items = new string[inventorySize];
        view = GetComponent<PhotonView>();
        myCamera.gameObject.SetActive(true);
        myCanvas.gameObject.SetActive(true);
        if(view.IsMine){
            CinemachineVirtualCamera myVCam = Instantiate(vCam, new Vector3(0, 0, 0), Quaternion.identity);
            myVCam.gameObject.SetActive(true);
            myVCam.Follow = gameObject.transform;
            myVCam.gameObject.transform.position = new Vector3(0, 0, 0);
            myVCam.Priority = 10;
            myVCam.gameObject.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = GameObject.Find("CameraBounds").GetComponent<PolygonCollider2D>();
        }
    }

    // Update is called once per frame.
    void Update()
    {   
        if(view.IsMine){
            if(Input.touchCount > 0){
                targetPos = new Vector2(myCamera.ScreenToWorldPoint(Input.GetTouch(0).position).x, myCamera.ScreenToWorldPoint(Input.GetTouch(0).position).y);
            }
            if(targetPos.x != transform.position.x || targetPos.y != transform.position.y && CR_running == false){
                if(targetPos.x > transform.position.x)
                    renderer.flipX = true;
                else
                    renderer.flipX = false;
                transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed*Time.deltaTime);
            }
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

        if(col.gameObject.tag == "playAreaBorder")
        {
            if((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 0 || (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 3 || (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 4)
            {
                col.gameObject.SetActive(false);
            }
            else
            {
                targetPos = new Vector2(transform.position.x, transform.position.y);
            }
        }

        if(col.gameObject.name == "ChangeSpeed")
        {
            if((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 0 || (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 3)
            {
                moveSpeed = moveSpeed * 0.667f;
            }
        }
        
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "ChangeSpeed")
        {
            if((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 0 || (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 3)
            {
                moveSpeed = moveSpeed * 1.5f;
            }
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
