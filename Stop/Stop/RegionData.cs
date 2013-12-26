using System;

public enum MouseLook
{
    DEFAULT_POINTER,
    LEFT_ARROW,
    RIGHT_ARROW,
    UP_ARROW,
    DOWN_ARROW,
    UPLEFT_ARROW,
    UPRIGHT_ARROW,
    DOWNLEFT_ARROW,
    DOWNRIGHT_ARROW,
    ACTION
}

public enum ConditionType
{
    INTRO, CLICK, CLICK_WITH_ITEM, CLIP_END
}

public enum EffectType
{
    LOAD, PLAY, SPRITE, GET_ITEM
}

public struct SceneData
{
    public RegionData[] regions;
    public string[] clips;
    public TriggerData[] triggers;
}

public struct Condition
{
    public ConditionType type;
    public string[] condition_args;
}

public struct Effect
{
    public EffectType type;
    public string[] effect_args;
}


public struct TriggerData
{
    public Condition[] conditions;
    public Effect[] effects;
}

public struct RegionData
{
    //values are percentages of total width/hight
    public double xPercent, widthPercent;
    public double yPercent, heightPercent;

    public MouseLook look;

    public String regionName;

    public override string ToString() { return regionName; }
}