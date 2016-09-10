using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

	public AudioClip clearSeClip;
	AudioSource clearSeAudio;

	void Start(){
		//オーディオの初期化
		clearSeAudio = gameObject.AddComponent<AudioSource>();
		clearSeAudio.loop = false;
		clearSeAudio.clip = clearSeClip;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
