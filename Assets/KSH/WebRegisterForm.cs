using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System;

public class WebRegisterForm : MonoBehaviour
{
    [Header("- 회원가입 정보")]
    public TMP_InputField registerIdInput;
    public TMP_InputField registerPwInput;
    public TMP_InputField registerNameInput;
    public TMP_InputField registerMailInput;

    private static readonly string PASSWORD = "wqe4r8da6s5f13z1cv89wer4713dsf";
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

    public void RegisterPost()
    {
        string plainId = registerIdInput.text;
        string plainPw = registerPwInput.text;
        string plainName = registerNameInput.text;
        string plainMail = registerMailInput.text;

        //암호화해서 회원가입, 키가 동일하므로 조회에서 큰 문제가 발생하진 않을 것으로 예상
        string inputPw = Encrypt(plainPw);
        StartCoroutine(Register(plainId, inputPw, plainName, plainMail));
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

    private IEnumerator Register(string _registerId, string _registerPw, string _registerName, string _registerMail)
    {
        //WWWForm form = new WWWForm();
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        /*
        form.AddField("reigsterId", _registerId);
        form.AddField("registerPw", _registerPw);
        form.AddField("registerName", _registerName);
        form.AddField("registerMail", _registerMail);
        /**/
        form.Add(new MultipartFormDataSection("StudentId", _registerId));
        form.Add(new MultipartFormDataSection("Pw", _registerPw));
        form.Add(new MultipartFormDataSection("StudentName", _registerName));
        form.Add(new MultipartFormDataSection("StudentMail", _registerMail));
        
        UnityWebRequest www = UnityWebRequest.Post("https://localhost:7295/api/Member/InsertUser", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("RegisterForm Uploaded");
        }
    }
}
