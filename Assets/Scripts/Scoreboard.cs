using System.IO
using UnityEngine;


namespace boards.Scoreboards
{
	public class Scoreboard : MonoBehaviour
	{
    	[SerializeField] private int maxScoreboardEntries = 4;
    	[SerializeField] private Transform highscoreHolderTransform = null;
    	[SerializeField] private GameObject scoreboardEntryObject = null;

    	[Header("Test")]
    	[SerializeField] ScoreboardEntryData testEntrydata = new ScoreboardEntryData();
	}
}

