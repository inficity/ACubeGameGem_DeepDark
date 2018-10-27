
using Newtonsoft.Json;

using System.Collections.Generic;

using UnityEngine.Networking;

namespace DeepDark
{
	public static class Messages
	{
		public static class Type
		{
			private const int BASE = 1000;

			public const int READY = BASE + 1;
		}

		public class EmptyMessage : MessageBase
		{
			//Empty.
		}

		public class JSONMessage : MessageBase
		{
			public string JSON;

			public void from<T>(T t)
			{
				this.JSON = JsonConvert.SerializeObject(t);
			}

			public T to<T>()
			{
				return JsonConvert.DeserializeObject<T>(this.JSON);
			}
		}
	}
}