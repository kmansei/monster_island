using UnityEngine; 
using System.Collections;

public class PlayerMove : MonoBehaviour {

	Vector3 direction; 
	//移動速度 
	public float move_speed = 5f; 
	//回転速度 
	public float rotate_speed = 180f; 
	//ジャンプ速度 
	public float jump_speed = 5f; 
	//重力 
	private float gravity=20f; 
	//アニメーターコンポーネント 
	Animator anim; 
	//キャラコントローラー 
	CharacterController chara;
	//アニメーション
	public CharaAnimation CharaAni;
	//ステータス
	CharacterStatus status;
	//コントローラー
	//PlayerCtrl playerctrl;

	Transform cam_trans; 
	// Use this for initialization 
	void Start () { 
		chara = GetComponent<CharacterController>(); 
		anim = GetComponentInChildren<Animator>(); 
		cam_trans = GameObject.Find("Main Camera").GetComponent<Transform>(); 
		CharaAni = GetComponent<CharaAnimation>();
		status = GetComponent<CharacterStatus>();
		//playerctrl = GetComponent<PlayerCtrl>();
	}

	// Update is called once per frame 
	void Update () {
		
		if (chara.isGrounded) 
		{

			//direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); 
			direction = (cam_trans.transform.right * Input.GetAxis("Horizontal")) + //transform.rightはX軸方向（右方）の単位ベクトル
				(cam_trans.transform.forward * Input.GetAxis("Vertical")); //Input.GetAxis("Horizontal")キーボードまたはジョイスティックの操作によって−1〜1までの数値を返す。

			//Debug.Log(direction.sqrMagnitude);

			if (direction.sqrMagnitude > 0.1f) //sqrMagnitude ベクトルの長さの2乗
			{ 
				Vector3 forward = Vector3.Slerp(transform.forward, direction, rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direction)); //2点間の角度を返す
				transform.LookAt(transform.position + forward); //LookAt その方向に向かせる
				/*
				public static Vector3 Slerp(Vector3 a, Vector3 b, float t);
				球状に2つのベクトル間を補間します
				tでaとbの間を補間します。この関数と、直線で補間する (別名 "leap")の違いは、ベクトルは方向としてではなく、空間の位置として扱われることです。 
				返されたベクトルの方向は角度によって補間され magnitude は from と to の大きさの間で補間されます。
				値は [0...1] の範囲に制限されます。 
				*/
			
			}

			if (Input.GetKeyDown (KeyCode.Space) && !status.attacking) { 
				//anim.SetBool ("is_Jump", true);
				direction.y = jump_speed;
			} else {
				//anim.SetBool ("is_Jump", false);
			}

			//if (chara.velocity.magnitude < 0.1 && Input.GetButton("Fire1")) { 
			if (Input.GetButton("Fire1")) { 
				anim.SetBool ("Attacking", true);
				CharaAni.attacked = true;
			} else {
				anim.SetBool ("Attacking", false);
				CharaAni.attacked = false;
			}

		}


		direction.y -= gravity * Time.deltaTime;
	
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
			chara.Move(direction * Time.deltaTime * move_speed); 
			anim.SetFloat("Speed", chara.velocity.magnitude);
			if(status.died){
				chara.Move(direction * Time.deltaTime * move_speed * 0); 
			}
		}



	}
		
} 