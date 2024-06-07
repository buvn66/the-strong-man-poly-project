using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DangNhapScrips : MonoBehaviour
{
    // Tạo ra hai biến để liên kết giao diện với Unity 
    public TMP_InputField edtEmail, edtPassword;
    public TMP_Text txtMessage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void KieuTraDangNhap()
    {
        // Lấy thông tin người dùng nhập vào input field 
        var email = edtEmail.text;
        var password = edtPassword.text;

        // So sánh với tài khoản 
<<<<<<< Updated upstream
        if (email.Equals("khang@gmail.com") && password.Equals("khang"))
=======
<<<<<<< HEAD
        if (email.Equals("kieuvu100@gmail.com") && password.Equals("kieuanhvu"))
=======
        if (email.Equals("khang@gmail.com") && password.Equals("khang"))
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
        {
            // Load màn chơi 
            SceneManager.LoadScene("Scene1 khang");
        }
        else
        {
            txtMessage.text = "EMAIL HOẶC MẬT KHẨU KHÔNG ĐÚNG";
        }
    }

    public void dangky()
    {
        // Chuyển đến cảnh đăng ký
        SceneManager.LoadScene("dangky");
    }

    public void menu()
    {
        // Chuyển đến cảnh đăng nhập
        SceneManager.LoadScene("menu");
    }

    public void QuitGame()
    {
        // Thoát trò chơi
        Application.Quit();

        // Đoạn mã này chỉ có tác dụng khi chạy trong Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
