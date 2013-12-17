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

public struct SceneData
{
    public RegionData[] regions;
    public string[] clips;
}

public struct RegionData
{
    //values are percentages of total width/hight
    public double xPercent, widthPercent;
    public double yPercent, heightPercent;

    public MouseLook look;

    public String eventName;
}