using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKey : MonoBehaviour
{

    public string _key;

    public string key
    {
        get { return _key; }
        set { value = _key; }
    }

    //버튼 푸쉬 시간
    public float ButtonPushTime;

    void Update()
    {
        //Setting.cs
        KeyMapping(); //키 맵핑
    }


    //키 맵핑
    public void KeyMapping()
    {
        #region A 
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerController.Player_Instance.player_att.GetAttackRange();
        }
        #endregion

        #region Spacebar (카메라 고정 계속)
        if (Input.GetKeyDown(KeyCode.Space) && gameManager.instance.Camera_Manager.CameraSet == true)
        {
            gameManager.instance.Camera_Manager.CameraSet = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space)) { gameManager.instance.Camera_Manager.CameraSet = true; }
        #endregion

        #region U (카메라 고정/풀기)
        if (Input.GetKeyDown(KeyCode.U))
        {
            gameManager.instance.Camera_Manager.CameraSet = (gameManager.instance.Camera_Manager.CameraSet == false ? true : false);
        }
        #endregion

        #region Mouse(1) (Move.cs)
        if (Input.GetMouseButtonDown(1))
        {
            PlayerController.Player_Instance.player_move.playerMove();
        }
        #endregion


        #region Keycode Q (Card.cs)
        if (Input.GetKey(KeyCode.Q))
        {
            ButtonPushTime += Time.deltaTime;
            if (ButtonPushTime >= 0.3) { Debug.Log("카드 정보"); } //꾹 누르면 카드 정보 ON
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(0);
                _key = "skill";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }
        #endregion

        #region Keycode W (Card.cs)
        if (Input.GetKey(KeyCode.W))
        {
            ButtonPushTime += Time.deltaTime;
            if (ButtonPushTime >= 0.3) { Debug.Log("카드 정보"); } //꾹 누르면 카드 정보 ON
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(1);
                _key = "skill";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }
        #endregion

        #region KeyCode E (Card.cs)
        if (Input.GetKey(KeyCode.E))
        {
            ButtonPushTime += Time.deltaTime;

            if (ButtonPushTime >= 0.3) { Debug.Log("카드 정보"); } //꾹 누르면 카드 정보 ON (탭 시간 기준 0.3초)
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(2);
                _key = "skill";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }
        #endregion

        #region Keycode R (Card.cs)
        if (Input.GetKey(KeyCode.R))
        {
            ButtonPushTime += Time.deltaTime;
            if (ButtonPushTime >= 0.3) { Debug.Log("카드 정보"); } //꾹 누르면 카드 정보 ON
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(3);
                _key = "skill";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }
        #endregion



        #region Keycode P (Setting.cs)
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameManager.instance.UI.GetStore("Store");
        }
        #endregion

        #region Keycode ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("옵션");
        }
        #endregion

        #region Keycode Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("캐릭터 정보");
        }

        #endregion

        #region Keycode B
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("귀환");
        }
        #endregion
    }

}
