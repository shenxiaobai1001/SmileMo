using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordingManager : MonoBehaviour
{
    public static RecordingManager Instance { get; private set; }

    [Header("录制存储")]
    public string saveFolder = "AutoRunRecordings";

    private Dictionary<string, RecordingSession> allRecordings =
        new Dictionary<string, RecordingSession>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveRecording(string name, RecordingSession recording)
    {
        allRecordings[name] = recording;

        // 序列化并保存到文件
        string json = JsonUtility.ToJson(recording, true);
        string path = GetRecordingPath(name);

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, json);

        Debug.Log($"录制已保存: {path}");
    }

    public RecordingSession LoadRecording(string name)
    {
        string path = GetRecordingPath(name);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            RecordingSession recording = JsonUtility.FromJson<RecordingSession>(json);
            allRecordings[name] = recording;
            return recording;
        }

        Debug.LogWarning($"录制文件不存在: {path}");
        return null;
    }

    public void LoadAllRecordings()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, saveFolder);

        if (!Directory.Exists(folderPath)) return;

        string[] files = Directory.GetFiles(folderPath, "*.json");

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                RecordingSession recording = JsonUtility.FromJson<RecordingSession>(json);
                string name = Path.GetFileNameWithoutExtension(file);
                allRecordings[name] = recording;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载录制文件失败: {file}, 错误: {e.Message}");
            }
        }
    }

    public RecordingSession GetBestRecordingForLevel(string levelName)
    {
        // 查找这个关卡的最佳录制
        RecordingSession bestRecording = null;
        float bestTime = float.MaxValue;

        foreach (var recording in allRecordings.Values)
        {
            if (recording.levelName == levelName && recording.duration < bestTime)
            {
                bestRecording = recording;
                bestTime = recording.duration;
            }
        }

        return bestRecording;
    }

    private string GetRecordingPath(string name)
    {
        return Path.Combine(Application.persistentDataPath, saveFolder, $"{name}.json");
    }
}