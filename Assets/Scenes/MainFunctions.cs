using Assets.Scenes.Games.BaseScene;
using System;
using System.IO;
using Assets.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainFunctions {

	public static readonly Game[] Games = {
		new Game("FastMath", null),
		new Game("FastCircles", null)
	};

	public static void LoadRandomGame() {
		SceneManager.LoadScene(Games[Random.Range(0, Games.Length)].Name);
	}

	public static void GameOver() {
		SceneManager.LoadScene("Scoreboard");
	}

	public static readonly string PathToData = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Data");

	public static int CalculateAddScore(int BaseScore, int TimeScore, float GivenTime, float TimeLeft) {
		if (BaseScore < 0) { throw new ArgumentOutOfRangeException(nameof(BaseScore)); }
		if (TimeScore < 0) { throw new ArgumentOutOfRangeException(nameof(TimeScore)); }

		float PercentTime = TimeLeft / GivenTime;
		TimeScore = (int)Math.Round(TimeScore * PercentTime);
		return BaseScore + TimeScore;
	}

}
