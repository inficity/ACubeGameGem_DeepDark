
using UnityEngine;
using UnityEngine.Networking;

namespace DeepDark
{
	public class Test : MonoBehaviour
	{
		private void Awake()
		{
			Debug.Log("NetworkServer.Listen : " + NetworkServer.Listen(8888));

			NetworkServer.RegisterHandler(MsgType.Connect, networkMessage =>
			{
				var message = new Messages.Message();
				var testMessage = new Messages.TestMessage();

				testMessage.fill();
				message.serialize(testMessage);

				NetworkServer.SendToClient(networkMessage.conn.connectionId, Messages.Type.TEST, message);
			});

			var client = new NetworkClient();
			client.RegisterHandler(Messages.Type.TEST, networkMessage =>
			{
				var message = networkMessage.ReadMessage<Messages.Message>().deserialize<Messages.TestMessage>();

				Debug.Log("network.a : " + message.a);
				Debug.Log("network.b : " + message.b);
				Debug.Log("network.c : " + message.c);
				Debug.Log("network.d.Count : " + message.d.Count);
				Debug.Log("network.e.Count : " + message.e.Count);
				Debug.Log("network.f.Count : " + message.f.Count);
				Debug.Log("network.g.Count : " + message.g.Count);
			});

			client.Connect("127.0.0.1", 8888);
		}
	}
}