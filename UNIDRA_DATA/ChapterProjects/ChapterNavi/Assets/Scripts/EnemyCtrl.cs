using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
	CharacterStatus status;
	CharaAnimation charaAnimation;
	CharacterMove characterMove;
	Transform attackTarget;
	public GameObject[] dropItemPrefab;

    public float waitBaseTime = 2.0f;
    float waitTime;
	public float walkRange = 5.0f;
    // 初期位置を保存しておく変数
    public Vector3 basePosition;

	// ステートの種類.
	enum State {
		Walking,
		Attacking,
		Chasing,
		Died,
	} ;
	
	State state = State.Walking;		// 現在のステート.
	State nextState = State.Walking;	// 次のステート.

	// Use this for initialization
	void Start () {
		status = GetComponent<CharacterStatus>();
		charaAnimation = GetComponent<CharaAnimation>();
		characterMove = GetComponent<CharacterMove>();
        basePosition = transform.position;
        waitTime = waitBaseTime;
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.Walking:
			Walking();
			break;
		case State.Chasing:
			Chasing();
			break;
		case State.Attacking:
			Attacking();
			break;
		case State.Died:
			Died();
			break;
		}
		
		if (state != nextState)
		{
			state = nextState;
			switch (state) {
			case State.Walking:
				WalkStart();
				break;
			case State.Chasing:
				ChaseStart();
				break;
			case State.Attacking:
				AttackStart();
				break;
			case State.Died:
				DiedStart();
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
		if( waitTime > 0.0f){
			waitTime -= Time.deltaTime;
			if (waitTime <= 0.0f){
				// 範囲内の何処か
				Vector2 randomValue = Random.insideUnitCircle * walkRange;
				// 移動場所の設定
                Vector3 destinationPosition = basePosition + (Vector3)(randomValue);

				SendMessage("SetDestination",destinationPosition);
			}
		}else{
			// 目的地へ到着
			if (characterMove.Arrived())
			{
				// 待機状態へ
				waitTime = Random.Range(waitBaseTime, waitBaseTime*2.0f);
			}
            if (attackTarget)
            {
                ChangeState(State.Chasing);
            }
        }
	}

	void ChaseStart()
	{
		StateStartCommon();
	}

	void Chasing()
	{
        // 目的地設定
        SendMessage("SetDestination", attackTarget.position);
        // 目的地へ到着
        if (Vector3.Distance( attackTarget.position, transform.position ) <= 2.0f)
		{
			ChangeState(State.Attacking);
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
		
	}
	
	// 攻撃中の処理.
	void Attacking()
	{
        if (charaAnimation.IsAttackFinished())
        {
            ChangeState(State.Walking);
            // 待機時間を再設定
            waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
            // ターゲットをリセットする
            attackTarget = null;
        }
	}

	void DiedStart()
	{
		status.died = true;
		dropItem();
		Destroy (gameObject,3.0f);		
	}
	
	void Died()
	{

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

	void dropItem()
	{
		if (dropItemPrefab.Length == 0)
			return;
		GameObject dropItem = dropItemPrefab[ Random.Range(0, dropItemPrefab.Length) ];
		Instantiate(dropItem, transform.position, transform.rotation);
	}

	public void SetAttackTarget(Transform target)
	{
		attackTarget = target;
	}
}
