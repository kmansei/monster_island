using UnityEngine;
using System.Collections;

public class EnemyGeneratorCtrl : MonoBehaviour {
	public GameObject enemyPrefab;
	
	GameObject[] existEnemys ;
	public int maxEnemy = 2;
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
                Vector3 pos = transform.position;
                // Raycastして地面に吸着させる
                RaycastHit hitInfo;
                if (Physics.Raycast(pos,Vector3.down,out hitInfo,1000.0f,(1 << LayerMask.NameToLayer("Ground"))))
                    pos = hitInfo.point;
                // 敵作成
                existEnemys[enemyCnt] = Instantiate(enemyPrefab, pos, transform.rotation) as GameObject;
                // 最大数を減らす
                totalEnemy--;
                return false;
            }
        }
        // 最大数がなくなったらコルーチン終了
        return (totalEnemy <= 0);
    }
}

