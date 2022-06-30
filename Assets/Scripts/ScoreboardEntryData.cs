using System;

namespace boards.Scoreboards
{
	//save data in a way to format to computer
	[Serializable]
	public struct ScoreboardEntryData
	{
    	public string entryName;
    	public int entryScore;
	}
}

