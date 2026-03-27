using System;
using System.Collections.Generic;

/// <summary>配置</summary>
public class Config
{
    public static int ClearType=1;
    public static int chainCount=0;
    public static int FlogCount;
    public static string NameValue;
    public static bool isSYSTEM = false;
    public static bool isFileAdd = true;
    public static bool LockPlayer=false;
    public static int ropeCount = 0;
    public static int succRopeCount = 0;
    public static int missRopeCount = 0; 
    public static int shineCount = 0;
    public static int kubaCount = 0;
    public static int hasKubaCount = 0;
    public static int eggCount = 0;
    public static int hasEggCount = 0;

}

public class ModData
{
    public static bool modDao = false;
    public static bool isProject =true;
    public static float freezeTime = 0;
    public static float protecketTime = 0;
    public static int chainCount = 0;
}

// 移动方向
public enum MoveDirection
{
    Left,
    Right
}   
// 移动类型
public enum MoveType
{
    None,
    Normal,
    NormalDuiKang,
    DuiKang,
    TC,
    QL
}

/// <summary>事件合集 </summary>
public enum Events
{
    None,
    GameRest,
    SaveSchedule,
    PlayerRestToSavePos,
    SevenBossDie,
    MapChanged,
    OnQLMove,
    OnTCMove,
    OnTrunckMove,
    OneFingerMove,
    RainBowCat,
    GaiyaTomato,
    GaiyaTomatoEnd,
    SevenMonsterDie,
    BeginSnakeMap,
    OnVideoPlayEnd,
    OnModVideoPlayEnd,
    AirWallStateChange,
    OnJingLi,
    OnLazzerHit,
    OnPlayerNameChange,
    OnBarryExecutEnd,
    HangSelfByKick,
    OnMangSengKick,
}

public enum PlayerControState
{
    None,
    LRun, 
    LRuning,
    CanelLRun,
    RRun,
    RRuning,
    CanelRRun,
    ToFast,
    Fast,
    CancelFast,
    Jump, 
    Jumping,
    CJump,
    LStickJump,
    RStickJump,
    Boost,
    Boosting,
    CanelBoost,
}
public enum PLState
{
    None,
    Idel,
    LRun,
    RRun,
    FastLRun,
    FastRRun,
    BrakeF,
    FLBrake,
    FRBrake,
    Jump,
    LJumping,
    FLJumping,
    RJumping,
    FRJumping,
    Drop,
    LDroping,
    FLDroping,
    RDroping,
    FRDroping,
    LStickDrop,
    RStickDrop,
    Stick,
    StickJump,
    LStick,
    RStick,
    LStickJump,
    RStickJump,
    StickF,
    StickFJump,
    LStickF,
    RStickF,
    LStickFJump,
    RStickFJump,
    LBoost,
    RBoost,
    HorHit,
    VecHit,
    DownHit,
    HitOver
}

public enum PlayerCheckType
{
    None,
    BackGround,
    LeftWall,
    RightWall,
}
public enum AutomaticMoveType
{
    None,
    Run,
    FastRun,
    Jump,
    Fly,
}
