using UnityEngine;
using System.Collections;

public class ArrowObject : MonoBehaviour {
	GameObject upArrow;
	GameObject downArrow;
	GameObject leftArrow;
	GameObject rightArrow;



	void  Start (){

	}

	void  Update (){

	}

	void  SetDir ( ArrayList dirList  ){
		upArrow.SetActive(false);
		downArrow.SetActive(false);
		leftArrow.SetActive(false);
		rightArrow.SetActive(false);
		for(int i = 0; i < dirList.length; i++){
			FIXME_VAR_TYPE str= dirList[i];
			if(str == "up"){
				upArrow.SetActive(true);
			}else if(str == "down"){
				downArrow.SetActive(true);
			}else if(str == "left"){
				leftArrow.SetActive(true);
			}else if(str == "right"){
				rightArrow.SetActive(true);
			}
		}
	}

	void  UpClick (){
		manager.UserSelectArrow("up");
	}
	void  DownClick (){
		manager.UserSelectArrow("down");
	}
	void  LeftClick (){
		manager.UserSelectArrow("left");
	}
	void  RightClick (){
		manager.UserSelectArrow("right");
	}
}