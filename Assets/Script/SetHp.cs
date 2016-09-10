using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetHp : MonoBehaviour {

	private CharacterStatus eStatus;
	private int count;
	private UnityEngine.UI.Slider slider;
	private int maxHp;
	public int countNow;

	// Use this for initialization
	void Start () {
		eStatus = transform.root.GetComponent<CharacterStatus>();
		slider = GetComponent<UnityEngine.UI.Slider>();
		maxHp = eStatus.HP;
		slider.value = slider.maxValue;
		count = maxHp;
	}
	
	// Update is called once per frame
	void Update () {
		countNow = eStatus.HP;

		if(countNow != count) {
			slider.value = (countNow * slider.maxValue) / maxHp;
			count = countNow;
		}

		if(eStatus.died) {
		GetComponent<CharacterController>().enabled = false;
		//GetComponentInChildren<RotateCamera>().Disable;
		}
  }
}