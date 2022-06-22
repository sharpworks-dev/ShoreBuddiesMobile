using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class RoomItem : MonoBehaviour
{
	public TMP_Text roomName;
	LobbyManager manager;

	void Awake(){
		roomName = GameObject.Find("Room Name").GetComponent<TMP_Text>();
	}
	
	public void Start()
	{
		manager = FindObjectOfType<LobbyManager>();
	}

	public void SetRoomName(string _roomName)
	{
		roomName.text = _roomName;
	}

	public void OnClickItem()
	{
		manager.JoinRoom(roomName.text);
	}
}
