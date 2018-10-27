
using System.Collections.Generic;

namespace DeepDark
{
	public class Global<T> where T : class
	{
		public Dictionary<int, T> Map { get; private set; }

		public Global(int firstId, int secondId, T first, T second)
		{
			this.Map = new Dictionary<int, T>();
			this.Map[firstId] = first;
			this.Map[secondId] = second;
		}
	}
}