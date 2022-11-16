using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    [Header("- 인트로 화면")]
    public GameObject mainIntro;
    [Header("- 로그인 화면")]
    public GameObject loginScene;
    [Header("- 회원가입 화면")]
    public GameObject registerScene;

    private void Start()
    {
        mainIntro.gameObject.SetActive(true);
    }

    public void LoginSceneChange()
    {
        loginScene.gameObject.SetActive(true);
        registerScene.gameObject.SetActive(false);
    }

    public void RegisterSceneChange()
    {
        loginScene.gameObject.SetActive(false);
        registerScene.gameObject.SetActive(true);
    }

    //메인 화면 가리고 로그인으로 넘어가기
    public void FalseIntro()
    {
        mainIntro.gameObject.SetActive(false);
    }

    public void LoadCustomScene()
    {
        SceneManager.LoadScene("CharacterCustomization");
    }
}
