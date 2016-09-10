using UnityEngine;
using System.Collections;

public class GameRuleCtrl : MonoBehaviour {
	// 残り時間
	public float timeRemaining = 5.0f * 60.0f;
	// ゲームオーバーフラグ
	public bool gameOver = false;
	// ゲームクリア
    public bool gameClear = false;
	// シーン移行時間
	public float sceneChangeTime = 3.0f;

	public AudioClip clearSeClip;
	AudioSource clearSeAudio;

	void Start()
	{
		// オーディオの初期化.
		clearSeAudio = gameObject.AddComponent<AudioSource>();
		clearSeAudio.loop = false;
		clearSeAudio.clip = clearSeClip;
	}

	void Update()
	{
		// ゲーム終了条件成立後、シーン遷移
		if( gameOver || gameClear ){
			sceneChangeTime -= Time.deltaTime;
			if( sceneChangeTime <= 0.0f ){
				Application.LoadLevel("TitleScene");
			}
			return;
		}

		timeRemaining -= Time.deltaTime;
		// 残り時間が無くなったらゲームオーバー
		if(timeRemaining<= 0.0f ){
			GameOver();
		}

	}
	
	public void GameOver()
	{
		gameOver = true;
	}
	public void GameClear()
	{
		gameClear = true;

		// オーディオ再生.
		clearSeAudio.Play ();
	}
}
