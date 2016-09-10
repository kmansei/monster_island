using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {
	public GameObject enemyPrefab;
	public float size = 3.0f;
	
	GameObject[] existEnemys ;
	public int maxEnemy = 1;
	public int totalEnemy = 1000000000;
	
	// Use this for initialization
	void Start () {
		existEnemys = new GameObject[maxEnemy];
		StartCoroutine(Exec());
	}


	IEnumerator Exec()
	{
		while(true){
			// yieldというキーワードで、中断させる事が出来ます
			// 再会するときは、yieldの後から実行されます。
			// ３秒に一度このプログラムが実行されます。
			if( Generator() )
				yield break;

            yield return new WaitForSeconds( 3.0f );
		}
	}

    bool Generator()
    {
        for (int enemyCnt = 0; enemyCnt < existEnemys.Length; ++enemyCnt)
        {
            if (existEnemys[enemyCnt] == null)
            {
                Vector3 pos = (Vector3)(Random.insideUnitCircle * size) + transform.position;
                // Raycastして地面に吸着させる
                RaycastHit hitInfo;
                if (Physics.Raycast(pos,Vector3.down,out hitInfo,1000.0f,(1 << LayerMask.NameToLayer("Ground"))))
                    pos = hitInfo.point;
                // 敵作成
                existEnemys[enemyCnt] = Instantiate(enemyPrefab,pos,Quaternion.Euler(0, Random.Range(-180.0f, 180.0f), 0)) as GameObject;
                // 最大数を減らす
                totalEnemy--;
                return false;
            }
        }
        // 最大数がなくなったらコルーチン終了
        return (totalEnemy <= 0);
    }
}

