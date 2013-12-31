using System;
using System.ComponentModel;

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
    LOAD_SCENE, START_SCENE, PLAY_CLIP, SPRITE, GET_ITEM, REMOVE_ITEM, SPECIAL
}

public class SceneData
{
    public RegionData[] regions;
    public string[] clips;
    public TriggerData[] triggers;
}

public class Condition : INotifyPropertyChanged
{
    public ConditionType type;
    public string[] condition_args;

    public String DisplayTitle
    {
        get
        {
            string title = type + "";
            if (condition_args != null)
            {
                foreach (string s in condition_args)
                    title += " " + s;
            }
            return title;
        }
    }
    public void OnDisplayPropertyChanged()
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
    }
    public event PropertyChangedEventHandler PropertyChanged;
}

public class Effect : INotifyPropertyChanged
{
    public EffectType type;
    public string[] effect_args;
    public String DisplayTitle
    {
        get
        {
            string title = type + "";
            if (effect_args != null)
            {
                foreach (string s in effect_args)
                    title += " " + s;
            }
            return title;
        }
    }
    public void OnDisplayPropertyChanged()
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
    }
    public event PropertyChangedEventHandler PropertyChanged;
}


public class TriggerData
{
    public Condition[] conditions;
    public Effect[] effects;
}

public class RegionData : INotifyPropertyChanged
{
    //values are percentages of total width/hight
    public double xPercent, widthPercent;
    public double yPercent, heightPercent;

    public MouseLook _look;

    public MouseLook look
    {
        get { return _look; }
        set { _look = value; OnDisplayPropertyChanged(); }
    }

    public string _name;

    public String regionName
    {
        get { return _name; }
        set { _name = value; OnDisplayPropertyChanged(); }
    }

    public override string ToString() { return regionName + " - " + _look; }

    public String DisplayTitle { get { return ToString(); } }

    void OnDisplayPropertyChanged()
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}