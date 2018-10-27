
using DeepDark;

using UnityEngine;
using UniRx;
using TimeSpan = System.TimeSpan;

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

		// Observable.Timer(TimeSpan.FromSeconds(30))
		// .Subscribe(_ => {

		// });
	}

	BooleanNotifier connectNotifier = new BooleanNotifier();
	BooleanNotifier readyNotifier = new BooleanNotifier();

	void Start()
	{
		connectNotifier.Throttle(TimeSpan.FromSeconds(2))
			.Subscribe(_ => {
				this.networkManager.connectServer(this.IP);
			});
		readyNotifier.Throttle(TimeSpan.FromSeconds(2))
			.Subscribe(_ => {
				this.networkManager.sendReady();
			});
	}

	public void OnConnectBtn()
	{
		connectNotifier.SwitchValue();
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


	public void OnReadyBtn()
	{
		readyNotifier.SwitchValue();
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
