using System.Collections;
using UnityEngine;

public class Camerawork : MonoBehaviour {

	GameObject chara;

	void  Start (){
		chara = GameObject.Find("Player");
	}

	void  Update (){
		if(chara != null){
			Vector3 movePos = new Vector3(chara.transform.position.x, chara.transform.position.y, transform.position.z);
			transform.position = Vector3.Lerp (transform.position, movePos, Time.deltaTime * 5);
		}else{
			chara = GameObject.Find("Player");
		}
	}
}
