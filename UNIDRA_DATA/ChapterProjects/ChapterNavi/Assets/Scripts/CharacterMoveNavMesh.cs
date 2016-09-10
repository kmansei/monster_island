using UnityEngine;
using System.Collections;

public class CharacterMoveNavMesh : MonoBehaviour {
	public NavMeshAgent agent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// 目的地を設定する.引数destinationは目的地.
	public void SetDestination(Vector3 destination)
	{
		agent.SetDestination( destination );
	}
}
