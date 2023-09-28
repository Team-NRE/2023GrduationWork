/// ksPark
///
/// Loading Page Matching System

using UnityEngine;
using UnityEngine.UI;
using Define;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_LoadingPage : UI_Scene
{
    private Image progressBar;

    // room custom property
    private string isRemainHuman = "H";
    private string isRemainCyborg = "C";
    private string roomStatus = "R";

    // player custom property
    private string team = "T";

    // Remain Team Num
    private int remainHuman, remainCyborg;
    
    public enum Buttons
    {
        Btn_ClosePopup1_White
    }

    public enum Images
	{
		Fill,
	}

	public override void Init()
	{
		Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        progressBar = Get<Image>((int)Images.Fill);
        GetButton((int)Buttons.Btn_ClosePopup1_White).gameObject.BindEvent(CancleMatch);

        QuickMatch();
    }

    public override void UpdateInit()
    {
        if (PhotonNetwork.CurrentRoom == null) return;
        if (Managers.game.isGameStart) return;
        
        SetProgressBar();
        FullRoomCheck();
    }

    public void CancleMatch(PointerEventData data)
    {
        Debug.Log("Return to Lobby");
        Managers.Clear();
        if (PhotonNetwork.CurrentRoom != null)
            PhotonNetwork.LeaveRoom();

        if (SceneManager.GetActiveScene().name == "View Test Scene")
            SceneManager.LoadScene("Lobby");
    }

    private void QuickMatch()
	{
        Debug.Log("- Quick Match Start");
        JoinRoom();
	}

    private void JoinRoom()
    {
        Debug.Log("- Join the Random Room");
        
        /// 입장 조건 (그 팀이 true일 때 입장 가능)
        /// 입장 실패 시, OnJoinRandomFailed() 실행됨.
        Hashtable customRoomProperties = new Hashtable();
        customRoomProperties.Add(Managers.game.myCharacterTeam.ToString().Substring(0, 1), true);

		PhotonNetwork.JoinRandomRoom(customRoomProperties, (byte)Managers.game.gameMode);
    }

    private void CreateRoom()
	{
        Debug.Log("- Create Room");

        /// 새로운 방 생성
		RoomOptions roomOptions = new RoomOptions() {
            MaxPlayers = (byte)Managers.game.gameMode,
            IsVisible = true,
            IsOpen = true,
            CustomRoomPropertiesForLobby = new string[] {isRemainHuman, isRemainCyborg},
            CustomRoomProperties = new Hashtable
            {
                {isRemainHuman, true},  // Human 팀 참가 가능 여부
                {isRemainCyborg, true},  // Cyborg 팀 참가 가능 여부
                {roomStatus, (int)RoomStatus.Waiting}  // 방 상황
            },
        };
		PhotonNetwork.CreateRoom(null, roomOptions, null);
	}

    private void SetPlayerProperties()
    {
        Debug.Log("- Player Properties Setting");

        /// 플레이어 프로퍼티 추가
        /// 플레이어가 소속된 팀
        /// 추가 성공 시, OnPlayerPropertiesUpdate() 실행됨.
        Hashtable customPlayerProperties = new Hashtable();
        customPlayerProperties.Add(team, (int)Managers.game.myCharacterTeam);

        PhotonNetwork.LocalPlayer.SetCustomProperties(customPlayerProperties);
    }

    /// 랜덤 룸 입장 실패 시, 실행됨
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Random Room : " + message);
        CreateRoom();
    }

    /// 방 입장 성공 시 실행
    public override void OnJoinedRoom()
    {
        SetPlayerProperties();

        if (!PhotonNetwork.IsMasterClient) return;

        /// 마스터 클라이언트만 초기화
        /// 총 방의 인원의 절반씩 설정
        remainHuman  = PhotonNetwork.CurrentRoom.MaxPlayers >> 1;
        remainCyborg = PhotonNetwork.CurrentRoom.MaxPlayers >> 1;
    }

    /// 플레이어 프로퍼티 추가 성공 시 실행됨.
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("- Player Properties End");
        if (!PhotonNetwork.IsMasterClient) return;
        
        PlayerTeam newPlayerTeam = (PlayerTeam)targetPlayer.CustomProperties[team];

        /// Human 팀일 경우
        if (newPlayerTeam == PlayerTeam.Human)
        {
            remainHuman--;

            if (remainHuman <= 0) 
            {
                ModifyCurrentRoomCustomProperties(isRemainHuman, false);
            }
        }

        /// Cyborg 팀일 경우
        if (newPlayerTeam == PlayerTeam.Cyborg)
        {
            remainCyborg--;

            if (remainCyborg <= 0)
            {
                ModifyCurrentRoomCustomProperties(isRemainCyborg, false);
            }
        }
    }

    private void ModifyCurrentRoomCustomProperties<T>(string name, T value)
    {
        if (!PhotonNetwork.InRoom) return;

        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;

        cp[name] = value;

        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
    }

    private void SetProgressBar()
    {
        progressBar.fillAmount = Mathf.MoveTowards(
            progressBar.fillAmount, 
            1.0f * PhotonNetwork.CurrentRoom.PlayerCount / PhotonNetwork.CurrentRoom.MaxPlayers, 
            Time.unscaledDeltaTime
        );
    }

    private void FullRoomCheck()
    {
        if (progressBar.fillAmount == 1.0f)
        {
            Managers.game.isGameStart = true;
        }
    }


}
