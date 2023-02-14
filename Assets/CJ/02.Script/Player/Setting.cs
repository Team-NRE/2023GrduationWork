using UnityEngine;


public partial class PlayerManager
{

    //키 맵핑
    public void KeyMapping()
    {
        #region Spacebar (Move.cs)
        if (Input.GetButton("CameraFixed"))
        {
            FixedCameraMove();
        }
        #endregion

        #region Mouse(1) (Move.cs)
        if (Input.GetMouseButtonDown(1))
        {
            PlayerMove();
        }
        #endregion



        #region Keycode Q (Card.cs)
        if (Input.GetButton("Q"))
        {
            ButtonPushTime += Time.deltaTime;
            if (ButtonPushTime >= 0.3) { gameManager.instance.UI.card.SetActive(true); } //꾹 누르면 카드 정보 ON
        }

        if (Input.GetButtonUp("Q"))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(0);
                KeyName = "Q";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }
        #endregion

        #region Keycode W (Card.cs)
        if (Input.GetButton("W"))
        {
            ButtonPushTime += Time.deltaTime;
            if (ButtonPushTime >= 0.3) { gameManager.instance.UI.card.SetActive(true); } //꾹 누르면 카드 정보 ON
        }

        if (Input.GetButtonUp("W"))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(1);
                KeyCode = "W";
                KeyName = "W";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }
        #endregion

        #region KeyCode E (Card.cs)
        if (Input.GetButton("E"))
        {
            ButtonPushTime += Time.deltaTime;

            if (ButtonPushTime >= 0.3) { gameManager.instance.UI.card.SetActive(true); } //꾹 누르면 카드 정보 ON (탭 시간 기준 0.3초)
        }

        if (Input.GetButtonUp("E"))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(2);
                KeyName = "E";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }
        #endregion

        #region Keycode R (Card.cs)
        if (Input.GetButton("R"))
        {
            ButtonPushTime += Time.deltaTime;
            if (ButtonPushTime >= 0.3) { gameManager.instance.UI.card.SetActive(true); } //꾹 누르면 카드 정보 ON
        }

        if (Input.GetButtonUp("R"))
        {
            if (ButtonPushTime < 0.3)
            {
                gameManager.instance.UI.UseCard(3);
                KeyName = "R";
            } //탭하면 SKill 사용

            ButtonPushTime = 0; //초기화
        }

        #endregion



        #region Keycode P (Setting.cs)
        if (Input.GetButtonDown("Store"))
        {
            gameManager.instance.UI.GetStore("Store");
        }
        #endregion



        #region Keycode ESC
        if (Input.GetButtonDown("Option"))
        {
            Debug.Log("옵션");
        }
        #endregion



        #region Keycode Tab
        if (Input.GetButtonDown("Info"))
        {
            Debug.Log("캐릭터 정보");
        }

        #endregion



        #region Keycode B
        if (Input.GetButtonDown("Home"))
        {
            Debug.Log("귀한");
        }
        #endregion


        
        #region Keycode A
        if (Input.GetButtonDown("Attack"))
        {
           Attack();
        }
        #endregion
    }

}



