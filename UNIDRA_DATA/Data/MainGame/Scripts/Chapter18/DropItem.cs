using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour {
	public enum ItemKind
	{
		Attack,
		Heal,
	};
	public ItemKind kind;

	// オーディオクリップ.
	public AudioClip itemSeClip;

	// 拾われたフラグ.
	bool isPickedUp = false;
	
	void OnTriggerEnter(Collider other)
	{	
		// Playerか判定.
		if( other.tag == "Player" ){
			// アイテム取得.
			CharacterStatus aStatus = other.GetComponent<CharacterStatus>();
			aStatus.GetItem(kind);
			// オーディオ再生.
			AudioSource.PlayClipAtPoint(itemSeClip,transform.position);
			// アイテムを取得をオーナーへ通知する.
			PlayerCtrl playerCtrl = other.GetComponent<PlayerCtrl>();
			if (playerCtrl.networkView.isMine) {
				if (networkView.isMine)
					GetItemOnNetwork(playerCtrl.networkView.viewID);
				else
					networkView.RPC("GetItemOnNetwork",networkView.owner,playerCtrl.networkView.viewID);
			}
		}
	}

	[RPC]
	// アイテム取得処理.
	void GetItemOnNetwork(NetworkViewID viewId)
	{
		// 拾われたフラグ.
		if (isPickedUp)
			return;
		isPickedUp = true;

		// 拾ったPlayerを探す.
		NetworkView player =  NetworkView.Find(viewId);
		if (player == null)
			return;

		// 拾ったPlayerにアイテムを与える.
		if (player.isMine)
			player.SendMessage("GetItem",kind);
		else 
			player.networkView.RPC("GetItem",player.owner,kind);


		Network.Destroy(gameObject);
		Network.RemoveRPCs(networkView.viewID);
	}
	
	void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		if (!networkView.isMine)
			Destroy(rigidbody);
	}

	// Use this for initialization
	void Start () {
		Vector3 velocity = Random.insideUnitSphere * 2.0f + Vector3.up * 8.0f;
		rigidbody.velocity = velocity;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
