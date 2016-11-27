namespace RandomSongPicker
{
	public class Song
	{
		public Song(string name, int page)
		{
			Name = name;
			Page = page;
		}
		public string Name { get; }
		public int Page { get; }

		#region Overrides of Object

		public override string ToString()
		{
			return $"{Name} (#{Page})";
		}

		#endregion
	}
}