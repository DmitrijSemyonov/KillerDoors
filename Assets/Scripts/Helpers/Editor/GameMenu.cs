using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameMenu : EditorWindow
{
    // ���� ����� �� �����, �.�. ����� ������ ��� ���� � ���� Edit. ������� "���� �������", "gffd gfg", Bibyter �� �������� � ���������
    [MenuItem("Game/Clear player prefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Game/Take screenshot")]
    public static void TakeScreenshot()
    {
        string path = "Screenshots";

        Directory.CreateDirectory(path);

        int i = 0;
        while (File.Exists(path + "/" + i + ".png")) i++;

        ScreenCapture.CaptureScreenshot(path + "/" + i + ".png");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeScreenshot();
        }
    }
}