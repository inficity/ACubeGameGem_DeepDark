
using DeepDark;

using UnityEngine;

public class NetworkUI : MonoBehaviour
{
	public string IP { get; set; }

	private NetworkManager networkManager;

	private void Awake()
	{
		this.networkManager = this.GetComponent<NetworkManager>();
	}

	public void OnConnectBtn()
	{
		this.networkManager.connectServer(this.IP);
	}

	public void OnServerBtn()
	{
		this.networkManager.runServer();
	}

	public void OnTestBtn()
	{
		this.gameObject.SetActive(false);
		DeepDark.Client.GameManager.Instance.TestGame();
	}
}
