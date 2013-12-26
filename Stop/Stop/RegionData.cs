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

public enum Condition
{
    INTRO, CLICK, CLICK_WITH_ITEM, CLIP_END
}

public enum Effect
{
    LOAD, PLAY, SPRITE, GET_ITEM
}

public struct SceneData
{
    public RegionData[] regions;
    public string[] clips;
    public Trigger[] triggers;
}

public struct Trigger
{
    public Condition condition;
    public string condition_args;
    public Effect effect;
    public string effect_args;
}

public struct RegionData
{
    //values are percentages of total width/hight
    public double xPercent, widthPercent;
    public double yPercent, heightPercent;

    public MouseLook look;

    public String regionName;
}