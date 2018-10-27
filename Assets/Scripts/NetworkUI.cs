using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUI : MonoBehaviour {

	public string IP {get; set;}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnConnectBtn() {

	}

	public void OnServerBtn() {

	}

	public void OnTestBtn() {
		this.gameObject.SetActive(false);
		DeepDark.Client.GameManager.Instance.TestGame();
	}
}
