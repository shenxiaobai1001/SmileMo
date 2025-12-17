using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum ActionType
{
    MoveLeft,
    MoveRight,
    Jump,
    Duck,
    Attack,
    Dash
}

[System.Serializable]
public class RecordedFrame
{
    public float timestamp;
    public Vector2 position;
    public Vector2 velocity;
    public ActionType[] actions;
    public int frameCount;
}

[System.Serializable]
public class RecordingSession
{
    public string levelName;
    public Vector3 startPosition;
    public int startFrame;
    public List<RecordedFrame> frames = new List<RecordedFrame>();
    public float duration;
}

public class AutoRunRecorder : MonoBehaviour
{
    [Header("录制设置")]
    public KeyCode startRecordKey = KeyCode.F5;
    public KeyCode stopRecordKey = KeyCode.F6;
    public KeyCode playRecordKey = KeyCode.F7;

    [SerializeField] private bool isRecording = false;
    [SerializeField] private bool isPlaying = false;

    private RecordingSession currentRecording;
    private PlayerController playerController;
    private Rigidbody2D playerRb;
    private int currentFrame = 0;

    // 保存多个录制片段
    private Dictionary<string, RecordingSession> savedRecordings =
        new Dictionary<string, RecordingSession>();

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody2D>();

        // 自动加载已保存的录制
        RecordingManager.Instance.LoadAllRecordings();
    }

    private void Update()
    {
        HandleRecordingControls();

        if (isRecording)
        {
            RecordCurrentFrame();
        }
    }

    private void FixedUpdate()
    {
        if (isPlaying && currentRecording != null)
        {
            PlaybackCurrentFrame();
        }
    }

    private void HandleRecordingControls()
    {
        // 开始录制
        if (Input.GetKeyDown(startRecordKey) && !isRecording && !isPlaying)
        {
            StartRecording();
        }

        // 停止录制
        if (Input.GetKeyDown(stopRecordKey) && isRecording)
        {
            StopRecording();
        }

        // 开始回放
        if (Input.GetKeyDown(playRecordKey) && !isRecording && !isPlaying)
        {
            StartPlayback();
        }
    }

    public void StartRecording()
    {
        isRecording = true;
        isPlaying = false;

        // 创建新的录制会话
        currentRecording = new RecordingSession
        {
            levelName = SceneManager.GetActiveScene().name,
            startPosition = transform.position,
            startFrame = Time.frameCount,
            frames = new List<RecordedFrame>()
        };

        // 记录初始状态
        RecordCurrentFrame();

        Debug.Log($"开始录制: {currentRecording.levelName} 在位置 {transform.position}");

        // 显示录制UI
        ShowRecordingUI(true);
    }

    private void RecordCurrentFrame()
    {
        if (!isRecording || currentRecording == null) return;

        // 收集当前帧的输入
        List<ActionType> currentActions = new List<ActionType>();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            currentActions.Add(ActionType.MoveLeft);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            currentActions.Add(ActionType.MoveRight);
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            currentActions.Add(ActionType.Jump);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            currentActions.Add(ActionType.Duck);
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.J))
            currentActions.Add(ActionType.Attack);
        if (Input.GetKey(KeyCode.LeftShift))
            currentActions.Add(ActionType.Dash);

        // 创建记录帧
        RecordedFrame frame = new RecordedFrame
        {
            timestamp = Time.time,
            position = transform.position,
            velocity = playerRb.velocity,
            actions = currentActions.ToArray(),
            frameCount = Time.frameCount
        };

        currentRecording.frames.Add(frame);
        currentRecording.duration = Time.time - (Time.time - (frame.timestamp - currentRecording.frames[0].timestamp));
    }

    public void StopRecording()
    {
        if (!isRecording || currentRecording == null) return;

        isRecording = false;
        currentRecording.duration = Time.time - (currentRecording.frames[0].timestamp);

        // 保存录制
        string recordingName = $"Recording_{currentRecording.levelName}_{DateTime.Now:yyyyMMdd_HHmmss}";
        RecordingManager.Instance.SaveRecording(recordingName, currentRecording);

        Debug.Log($"停止录制: {recordingName}, 时长: {currentRecording.duration:F2}秒, 帧数: {currentRecording.frames.Count}");

        ShowRecordingUI(false);
    }

    public void StartPlayback()
    {
        if (isRecording || currentRecording == null)
        {
            Debug.LogWarning("没有可用的录制数据");
            return;
        }

        isPlaying = true;
        isRecording = false;
        currentFrame = 0;

        // 禁用玩家输入控制
        if (playerController != null)
            playerController.enabled = false;

        Debug.Log($"开始回放: {currentRecording.levelName}, 总帧数: {currentRecording.frames.Count}");

        ShowPlaybackUI(true);
    }

    private void PlaybackCurrentFrame()
    {
        if (!isPlaying || currentRecording == null || currentFrame >= currentRecording.frames.Count)
        {
            StopPlayback();
            return;
        }

        RecordedFrame frame = currentRecording.frames[currentFrame];

        // 应用位置和速度
        transform.position = frame.position;
        playerRb.velocity = frame.velocity;

        // 执行动作
        foreach (ActionType action in frame.actions)
        {
            ExecuteAction(action);
        }

        currentFrame++;
    }

    private void ExecuteAction(ActionType action)
    {
        // 这里触发相应的动作
        switch (action)
        {
            case ActionType.MoveLeft:
                playerRb.velocity = new Vector2(-5, playerRb.velocity.y);
                break;
            case ActionType.MoveRight:
                playerRb.velocity = new Vector2(5, playerRb.velocity.y);
                break;
            case ActionType.Jump:
                if (IsGrounded())
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 10);
                break;
                // 添加其他动作...
        }
    }

    private bool IsGrounded()
    {
        // 简单的接地检测
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
    }

    public void StopPlayback()
    {
        isPlaying = false;

        // 重新启用玩家控制
        if (playerController != null)
            playerController.enabled = true;

        Debug.Log("停止回放");

        ShowPlaybackUI(false);
    }

    private void ShowRecordingUI(bool show)
    {
        // 这里可以添加UI显示
       // DebugUI.Instance?.ShowMessage(show ? "录制中..." : "");
    }

    private void ShowPlaybackUI(bool show)
    {
        // 这里可以添加UI显示
       // DebugUI.Instance?.ShowMessage(show ? "回放中..." : "");
    }
}