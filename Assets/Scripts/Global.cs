
namespace DeepDark
{
	public class Global<T> where T : class
	{
		public T Top { get; private set; }
		public T Bottom { get; private set; }

		public Global(T top, T bottom)
		{
			this.Top = top;
			this.Bottom = bottom;
		}
	}
}