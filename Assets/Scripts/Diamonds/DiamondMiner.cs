using System;
using System.Globalization;
using UnityEngine;

public class DiamondMiner : MonoBehaviour
{
    private const string StartTimeKey = "StartTime";

    private const string AppQuitTimeKey = "AppQuitTime";

    private const string DiamondsCountKey = "DiamondsCount";

    private const float DiamondsPerSec = 1f / 36f; // equals 100 diamonds per hour

    private bool isOriginalMiner = false;

    private int lastSecond;

    [SerializeField]
    private bool clearData = false; //set to true in editor to clear all data, don't forget set to false after first run

    private void Awake()
    {
        if (this.clearData)
        {
            PlayerPrefs.DeleteAll();
        }

        if (Time.time.Equals(0f))
        {
            this.isOriginalMiner = true;
        }

        if (!PlayerPrefs.HasKey(StartTimeKey))
        {
            PlayerPrefs.SetString(StartTimeKey, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetFloat(DiamondsCountKey, 0);
        }
        else
        {
            this.CheckOfflineDiamonds();
        }

        this.lastSecond = DateTime.UtcNow.Second;
        this.PrintInConsole(PlayerPrefs.GetFloat(DiamondsCountKey).ToString());
    }

    private void Update()
    {
        float diamondsPerFrame = DiamondsPerSec * Time.deltaTime;
        float diamondsCount = PlayerPrefs.GetFloat(DiamondsCountKey);
        diamondsCount += diamondsPerFrame;
        PlayerPrefs.SetFloat(DiamondsCountKey, diamondsCount);
        if (DateTime.UtcNow.Second != this.lastSecond) //print the value of diamonds every exact clock second (not application run time) to the console
        {
            this.PrintInConsole(PlayerPrefs.GetFloat(DiamondsCountKey).ToString());
            this.lastSecond = DateTime.UtcNow.Second;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(AppQuitTimeKey, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
    }

    private void CheckOfflineDiamonds()
    {
        if (!PlayerPrefs.HasKey(AppQuitTimeKey)) return;
        DateTime quitTime = this.GetDateFromString(AppQuitTimeKey);
        TimeSpan span = DateTime.UtcNow - quitTime;
        float diamondsWhileAppClosed = (span.Milliseconds / 1000f) * DiamondsPerSec; //we are using milliseconds here for more precision
        float diamondsCount = PlayerPrefs.GetFloat(DiamondsCountKey);
        diamondsCount += diamondsWhileAppClosed;
        PlayerPrefs.SetFloat(DiamondsCountKey, diamondsCount);
        this.PrintInConsole("Mined offline: " + diamondsWhileAppClosed);
    }

    private DateTime GetDateFromString(string dateString)
    {
        return DateTime.Parse(PlayerPrefs.GetString(dateString));
    }

    private void PrintInConsole(string message)
    {
        if (!this.isOriginalMiner) return; //only original miner that was on scene from start can print in console
        Debug.Log(message);
    }
}