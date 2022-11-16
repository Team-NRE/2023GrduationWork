using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System;
using Photon.Chat;

/*
    !주의사항
    WWWForm은 레거시 코드이므로 차후 개선의 필요성이 있음 
*/

public class LoginInfo
{
    public static string userId;
    public static string NamePickUserName;
}

public class WebLoginForm : MonoBehaviour
{
    private static readonly string PASSWORD = "wqe4r8da6s5f13z1cv89wer4713dsf";
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

    [Header("- 로그인 학번")]
    public TMP_InputField studentIdInput;
    [Header("- 로그인 비밀번호")]
    public TMP_InputField pwInput;
    [Header("- 로그인 학생명, 채팅명으로 사용")]
    public TMP_InputField nameInput;

    //코루틴으로 로그인을 돌려버리는 메서드
    public void LoginPost()
    {
        //inputField 받아오기
        string plainId = studentIdInput.text;
        string plainPw = pwInput.text;

        //출석부용 정보 저장
        LoginInfo.userId = plainId;
        LoginInfo.NamePickUserName = nameInput.text;
        PlayerPrefs.SetString("Name", LoginInfo.NamePickUserName);

        //암호화해서 서버로 넘긴다. 아이디는 평문으로 해도 문제가 없다고 판단
        string inputPw = Encrypt(plainPw);
        StartCoroutine(Login(plainId, inputPw));
        Debug.Log(inputPw);
        Debug.Log("서버 통신 개시");
    }

    //암호화 메서드
    private string Encrypt(string plainString)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainString);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = myRijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();
        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    //복호화 함수
    private string Decrypt(string encryptString)
    {
        byte[] encryptBytes = Convert.FromBase64String(encryptString);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();
        ICryptoTransform decryptor = myRijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        cryptoStream.Close();

        return plainString;
    }

    public IEnumerator Login(string _studentId, string _pw)
    {
        //WWWForm form = new WWWForm();
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        /*
        form.AddField("studentId", _studentId);
        form.AddField("loginPw", _pw);
        */
        form.Add(new MultipartFormDataSection("StudentId", _studentId));
        form.Add(new MultipartFormDataSection("Pw", _pw));

        UnityWebRequest www = UnityWebRequest.Post("https://localhost:7295/api/Member/LoginUser", form);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("LoginForm Uploaded");
            LoginInfo.userId = _studentId;
            SceneManager.LoadScene("CharacterCustomization");
        }
    }
}
