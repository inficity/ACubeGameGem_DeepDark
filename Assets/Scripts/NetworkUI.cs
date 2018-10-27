
using DeepDark;

using UnityEngine;

public class NetworkUI : MonoBehaviour
{
	public static NetworkUI Instance;
	public string IP { get; set; }

	private NetworkManager networkManager;

	private void Awake()
	{
		Instance = this;
		this.networkManager = this.GetComponent<NetworkManager>();
		IP = "192.168.13.121";
		ShowReadyUI(false);
		ShowReadyUI(false);
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

	public GameObject ConnectionUI;
	public void ShowConnectUI(bool visible)
	{
		ConnectionUI.SetActive(visible);
	}
	public GameObject ReadyUI;
	public void ShowReadyUI(bool visible)
	{
		ReadyUI.SetActive(visible);
	}
}
