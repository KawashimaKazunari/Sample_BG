using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {
	public GameObject  diceObject;
	public bool diceStartFlg;


	void  Update (){

		if(gameStatus == 1 && Input.GetMouseButtonDown(0)){
			ChangeGameStatus(2);
			diceStartFlg = true;
			diceObject.rigidbody.AddForce(Force(),ForceMode.Impulse);
			diceObject.rigidbody.AddTorque(new Vector3(-50 * Random.value * diceObject.transform.localScale.magnitude, -50 * Random.value * diceObject.transform.localScale.magnitude, -50 * Random.value * diceObject.transform.localScale.magnitude), ForceMode.Impulse);

		}

		// Dice転がり終了
		if(diceStartFlg){
			if(diceObject.rigidbody.velocity.magnitude == 0){
				diceStartFlg = false;
				CheckDice();
			}

		}
	}

	void  Force (){
		Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
		return Vector3.Lerp(diceObject.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
	}
}
