using System;
using System.Collections.Generic;


namespace boards.Scoreboards
{
	[Serializable]
	public class ScoreboardSaveData
	{
    	public List<ScoreboardEntryData> highscores = new List<ScoreboardEntryData>();
	}
}

