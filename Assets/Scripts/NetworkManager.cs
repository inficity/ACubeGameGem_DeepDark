
using UnityEngine;
using UnityEngine.Networking;

namespace DeepDark
{
	public class NetworkManager : MonoBehaviour
	{
		public enum Type
		{
			Client,
			Server
		}

		public static NetworkManager Instance { get; private set; }

		public Type _Type;

		private void Awake()
		{
			NetworkManager.Instance = this;

			switch(this._Type)
			{
				case Type.Client:

					break;
				case Type.Server:
					NetworkServer.Listen(8888);
					break;
			}
		}
	}
}