using System.Collections;
using UnityEngine;
public class SugorokuManage : MonoBehaviour {
	int masuRowSize;
	int masuColSize;
	ArrayList[] masuList;         // マスの配列
	ArrayList[] masuObjectList;   // マスオブジェクトの配列
	int gameStatus;         // 0:wait 1:waitDiceClick 2:diceWait 3:playerMenuSelectWait 4:playerMove 5:userWaySelect 6:clear
	GameObject mainCamera;

	GameObject chara;
	//-------------------- UIまわり ------------------
	GameObject UICamera;
	GameObject menuObject;
	GameObject limitMoveText;
	int limitValue; // 実際に動ける残りの数
	GameObject allows;  // →

	//-------------------- ステージ -----------------
	GameObject startObject;
	GameObject goalObject;
	Vector2 startPos;
	Vector2 goalPos;
	//-------------------- さいころ ------------------
	GameObject diceCamera;
	GameObject diceObject;  // さいころ
	bool  diceStartFlg;   // さいころ転がり中フラグ

	//-------------------- ターン -----------------
	int diceValue;      // 出た目

	//-------------------- Clear ----------------
	GameObject clearText;
	//その他
	int charaDir;

	void  Start (){

		ChangeGameStatus(0);
		diceStartFlg = false;

		StageCreate();

		CharaCreate();

		ChangeGameStatus(3);
	}

	void  ChangeGameStatus ( int status  ){
		gameStatus = status;
		if(status == 0){
		}

		if(status == 1 || status == 2){
			if(status == 1){
				diceObject.transform.position = Vector3(3, 1, -8);
			}
			diceCamera.SetActive(true);
		}else{
			diceCamera.SetActive(false);
		}

		if(status == 3){
			menuObject.SetActive(true);
		}else{
			menuObject.SetActive(false);
		}

		if(status == 4 || status == 5){
			limitMoveText.SetActive(true);
			if(status == 4){
				limitMoveText.GetComponent<TextMesh>().text = "残り"+ diceValue + "マス";
			}
		}else{
			limitMoveText.SetActive(false);
		}

		if(status == 5){
			allows.SetActive(true);
		}else{
			allows.SetActive(false);
		}

		if(status == 6){
			Vector3 pos = chara.transform.position;
			pos.y += 2;
			clearText.transform.position = pos;
			clearText.SetActive(true);
		}else{
			clearText.SetActive(false);
		}
	}

	// キャラクター移動  
	private void  CharaMove (){
		yield return new WaitForSeconds(0.2f);



		//while(limitValue > 0){
		if(limitValue <= 0){
			ChangeGameStatus(3);
		}else{
			// 現在位置から自分が来た方向以外の方向のますをチェック

			bool  upFlg = false;
			bool  downFlg = false;
			bool  leftFlg = false;
			bool  rightFlg = false;
			int count = 0;
			ArrayList nextPosList = new ArrayList();
			ArrayList nextDirList = new ArrayList();

			if(charaDir != "left" && pos.x != masuColSize - 1 && GetMasuData(pos.x + 1, pos.y) != "#"){
				count++;
				upFlg = true;
				nextPosList.push (new Vector2 (pos.x + 1, pos.y));
				nextDirList.push("right");
				//Debug.Log("右にいける");
			}

			if(charaDir != "right" && pos.x != 0 && GetMasuData(pos.x - 1, pos.y) != "#"){
				count++;
				downFlg = true;
				nextPosList.push(new Vector2(pos.x - 1, pos.y));
				nextDirList.push("left");
				//Debug.Log("左にいける");
			}

			if(charaDir != "down" && pos.y != 0 && GetMasuData(pos.x, pos.y - 1) != "#"){
				count++;
				rightFlg = true;
				nextPosList.push(new Vector2(pos.x, pos.y - 1));
				nextDirList.push("up");
				//Debug.Log("上にいける");
			}

			if(charaDir != "up" && pos.y != masuRowSize - 1  && GetMasuData(pos.x, pos.y + 1) != "#"){
				count++;
				leftFlg = true;
				nextPosList.push(new Vector2(pos.x, pos.y + 1));
				nextDirList.push("down");
				//Debug.Log("下にいける");
			}

			// 自動で進む
			if(count == 1){
				limitValue--;
				limitMoveText.GetComponent<TextMesh>().text = "残り"+ limitValue + "マス";
				Vector2 nextPos = nextPosList[0];
				GameObject nextMasuObject = GameObject.Find("Masu" + nextPos.x + nextPos.y);
				iTween.MoveTo(chara, iTween.Hash("x", nextMasuObject.transform.position.x, "y", nextMasuObject.transform.position.y,"delay", 0, "time", 0.5f, "oncomplete", "CharaMove", "oncompletetarget", this.gameObject));

				charaObject.dir = nextDirList[0];
				charaObject.pos = nextPosList[0];

			}else{
				// ユーザーに選んでもらう
				allows.transform.position = chara.transform.position;
				allows.GetComponent<ArrowObject>().SetDir(nextDirList);
				ChangeGameStatus(5);
			}
		}
	}

	// キャラクター配置
	private void  CharaCreate (){
		chara = Instantiate(Resources.Load("Prefabs/Object/Chara"));
		chara.transform.position = startObject.transform.position;
		chara.name = "Player";
		Player charaObject = chara.GetComponent<Player>();
		charaObject.pos = startPos;
		charaObject.dir = "up";
	}

	private void  GetMasuData ( int x ,   int y  ){
		return masuList[x + y * masuColSize];
	}

	// ステージの配置
	private void  StageCreate (){

		masuObjectList = new Array();
		masuList = new Array();
		Array stageStrList = DataBase.Stage1;
		masuRowSize = stageStrList.length;
		for(int y = 0; y < stageStrList.length; y++){
			string str = stageStrList[y];
			masuColSize = str.Length;
			for(int x = 0; x < str.Length; x++){
				string masuStr = str.Substring(x, 1);
				GameObject masuObject = null;
				masuList[x + y * str.Length] = masuStr;
				if(masuStr != "#"){
					masuObject = Instantiate(Resources.Load("Prefabs/Object/Masu"), new Vector3(1.0f* x, -1.0f * y, 0)  ,Quaternion.identity);
					masuObject.name = "Masu"+ x + y;
				}
				if(masuStr == "#"){
				}else if(masuStr == "0"){
				}else if(masuStr == "S"){
					tk2dSprite tex = masuObject.GetComponent<tk2dSprite>();
					tex.color = Color.blue;
					mainCamera.transform.position.x = masuObject.transform.position.x;
					mainCamera.transform.position.y = masuObject.transform.position.y;
					startObject = masuObject;
					startPos = new Vector2(x,y);
				}else if(masuStr == "G"){
					tex = masuObject.GetComponent<tk2dSprite>();
					tex.color = Color.red;
					goalObject = masuObject;
					goalPos = new Vector2(x,y);
				}
				masuObjectList.push(masuObject);
			}
		}
	}
	void  Force (){
		Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
		return Vector3.Lerp(diceObject.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
	}

	void  Update (){

		if(gameStatus == 1 && Input.GetMouseButtonDown(0)){
			ChangeGameStatus(2);
			diceStartFlg = true;
			//diceObject.rigidbody.AddExplosionForce(00, diceObject.transform.position - Vector3.left * 3.0f, 10, 3.0f);
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

	// ユーザーの方向選択完了イベント
	void  UserSelectArrow ( string dir  ){
		ChangeGameStatus(4);

		// 移動
		limitValue--;
		limitMoveText.GetComponent<TextMesh>().text = "残り"+ limitValue + "マス";
		Player charaObject = chara.GetComponent<Player>();
		Vector2 pos = charaObject.pos;
		Vector2 nextPos;
		if(dir == "left"){
			nextPos = new Vector2(pos.x - 1, pos.y);
		}else if(dir == "right"){
			nextPos = new Vector2(pos.x + 1, pos.y);
		}else if(dir == "up"){
			nextPos = new Vector2(pos.x, pos.y -1);
		}else if(dir == "down"){
			nextPos = new Vector2(pos.x, pos.y + 1);
		}

		GameObject nextMasuObject = GameObject.Find("Masu" + nextPos.x + nextPos.y);
		iTween.MoveTo(chara, iTween.Hash("x", nextMasuObject.transform.position.x, "y", nextMasuObject.transform.position.y,"delay", 0, "time", 0.7f, "oncomplete", "CharaMove", "oncompletetarget", this.gameObject));

		charaObject.dir = dir;
		charaObject.pos = nextPos;
	}

	void  CheckDice (){
		diceValue = 0;
		while(diceValue == 0){
			diceValue = diceObject.GetComponent<Die>().value;

			if(diceValue != 0){
				yield return new WaitForSeconds(1.5f);
				diceValue = diceObject.GetComponent<Die>().value;
				limitValue = diceValue;
				ChangeGameStatus(4);
				CharaMove();
			}else{
				// 判定できない場合もうちょっと転がす
				diceObject.rigidbody.AddForce(Force(),ForceMode.Impulse);
				yield return new WaitForSeconds(2.0f);
			}
		}
	}


	void  ClickDiceBtn (){
		ChangeGameStatus(1);
	}
}