using UnityEngine;
using System.Collections;

public class GameRuleCtrl : MonoBehaviour {
	// 残り時間
	public float timeRemaining = 5.0f * 60.0f;
	// ゲームオーバーフラグ
	public bool gameOver = false;
	// ゲームクリア
    public bool gameClear = false;

	void Update()
	{
		timeRemaining -= Time.deltaTime;
		// 残り時間が無くなったらゲームオーバー
		if(timeRemaining<= 0.0f ){
			GameOver();
		}
	}
	
	public void GameOver()
	{
		gameOver = true;
        Debug.Log("GameOver");
	}
	public void GameClear()
	{
		gameClear = true;
        Debug.Log("GameClear");
    }
}
