using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	const float RayCastMaxDistance = 100.0f;
	CharacterStatus status;
	CharaAnimation charaAnimation;
	Transform attackTarget;
	InputManager inputManager;
	public float attackRange = 1.5f;
	
	// ステートの種類.
	enum State {
		Walking,
		Attacking,
		Died,
	} ;
	
	State state = State.Walking;		// 現在のステート.
	State nextState = State.Walking;	// 次のステート.
	
	
	// Use this for initialization
	void Start () {
		status = GetComponent<CharacterStatus>();
		charaAnimation = GetComponent<CharaAnimation>();
		inputManager = FindObjectOfType<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.Walking:
			Walking();
			break;
		case State.Attacking:
			Attacking();
			break;
		}
		
		if (state != nextState)
		{
			state = nextState;
			switch (state) {
			case State.Walking:
				WalkStart();
				break;
			case State.Attacking:
				AttackStart();
				break;
			case State.Died:
				Died();
				break;
			}
		}
	}
	
	
	// ステートを変更する.
	void ChangeState(State nextState)
	{
		this.nextState = nextState;
	}
	
	void WalkStart()
	{
		StateStartCommon();
	}
	
	void Walking()
	{
		if (inputManager.Clicked()) {
			// RayCastで対象物を調べる.
			Ray ray = Camera.main.ScreenPointToRay(inputManager.GetCursorPosition());
			RaycastHit hitInfo;
			if (Physics.Raycast(ray,out hitInfo,RayCastMaxDistance,(1<<LayerMask.NameToLayer("Ground"))|(1<<LayerMask.NameToLayer("EnemyHit")))) {
				// 地面がクリックされた.
				if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
					SendMessage("SetDestination",hitInfo.point);
				// 敵がクリックされた.
				if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("EnemyHit")) {
					// 水平距離をチェックして攻撃するか決める.
					Vector3 hitPoint = hitInfo.point;
					hitPoint.y = transform.position.y;
					float distance = Vector3.Distance(hitPoint,transform.position);
					if (distance < attackRange) {
						// 攻撃.
						attackTarget = hitInfo.collider.transform;
						ChangeState(State.Attacking);
					} else
						SendMessage("SetDestination",hitInfo.point);
				}
			}
		}
	}
	
	// 攻撃ステートが始まる前に呼び出される.
	void AttackStart()
	{
		StateStartCommon();
		status.attacking = true;
		
		// 敵の方向に振り向かせる.
		Vector3 targetDirection = (attackTarget.position-transform.position).normalized;
		SendMessage("SetDirection",targetDirection);
		
		// 移動を止める.
		SendMessage("StopMove");
	}
	
	// 攻撃中の処理.
	void Attacking()
	{
		if (charaAnimation.IsAttacked())
			ChangeState(State.Walking);
	}
	
	void Died()
	{
		status.died = true;
	}
	
	void Damage(AttackArea.AttackInfo attackInfo)
	{
		status.HP -= attackInfo.attackPower;
		if (status.HP <= 0) {
			status.HP = 0;
			// 体力０なので死亡ステートへ.
			ChangeState(State.Died);
		}
	}
	
	// ステートが始まる前にステータスを初期化する.
	void StateStartCommon()
	{
		status.attacking = false;
		status.died = false;
	}
}
