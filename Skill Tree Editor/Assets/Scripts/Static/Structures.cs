using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml.Linq;
using Unity.VisualScripting;

public readonly struct NodeData
{
    public static NodeData Empty = new(0, "", "", "", Array.Empty<RewardData>(), -1);

    [DefaultValue(0)] //probably not needed
    public readonly int Type;

    public readonly string Id;

    public readonly string Title;
    public readonly string Description;

    public readonly RewardData[] Rewards;
    public readonly int StartClass;
    public NodeData(int type, string id, string title, string description, RewardData[] rewards, int startClass = -1)
    {
        Type = type;
        Id = id;
        Title = title;
        Description = description;
        Rewards = rewards;
        StartClass = startClass;

        if (Id == null || Id.Length == 0)
            Id = $"Skill Node {Type}";
    }
    public NodeData(XElement data)
    {
        Type = data.ParseInt("@type");
        Id = data.ParseString("@id", $"Skill Node {Type}");
        Title = data.ParseString("DisplayTitle", string.Empty);
        Description = data.ParseString("Description", string.Empty);

        var list = new List<RewardData>();
        foreach(var rewardXml in data.Elements("Reward"))
        {
            list.Add(new RewardData(rewardXml));
        }
        Rewards = list.ToArray();

        StartClass = data.ParseInt("StartClass", -1);
    }
    public XElement Export(XElement data)
    {
        data.Add(new XAttribute("type", Type));

        if (Id == null)
            data.Add(new XAttribute("id",$"Skill Node {Type}"));
        else
            data.Add(new XAttribute("id", Id));
        
        data.Add(new XElement("DisplayTitle", Title));
        data.Add(new XElement("Description", Description));
        data.Add(new XElement("StartClass", StartClass));

        foreach(var reward in Rewards)
        {
            data.Add(reward.Export());
        }

        return data;
    }
}
public struct RewardData
{
    public static RewardData Empty = new(NodeReward.None, 0, 0, false);
    //What reward type
    public readonly NodeReward Reward;
    //What reward inside of the type
    public readonly int RewardIndex;
    //amount of that reward
    public readonly int RewardAmount;
    public readonly bool IsPercentage;
    public readonly int ExtraInt;
    public RewardData(NodeReward reward, int rewardIndex, int rewardAmount, bool isPercentage, int extraInt = -1)
    {
        Reward = reward;
        RewardIndex = rewardIndex;
        RewardAmount = rewardAmount;
        IsPercentage = isPercentage;
        ExtraInt = extraInt;
    }
    public RewardData(XElement data)
    {
        Reward = (NodeReward)data.ParseInt("NodeReward");
        RewardIndex = data.ParseInt("RewardIndex");
        RewardAmount = data.ParseInt("RewardAmount");
        IsPercentage = data.ParseBool("IsPercentage");
        ExtraInt = data.ParseInt("ExtraInt", -1);
    }
    public XElement Export()
    {
        XElement data = new XElement("Reward");
        data.Add(new XElement("NodeReward", (int)Reward));
        data.Add(new XElement("RewardIndex", RewardIndex));
        data.Add(new XElement("RewardAmount", RewardAmount));
        data.Add(new XElement("IsPercentage", IsPercentage));

        if(ExtraInt != -1)
            data.Add(new XElement("ExtraInt", ExtraInt));

        return data;
    }

    public bool IsEmpty()
    {
        if (Reward == NodeReward.None && RewardIndex == 0 && RewardAmount == 0 && !IsPercentage && ExtraInt == -1)
            return true;

        return false;
    }
}
public struct ConnectionData
{
    public static ConnectionData Empty = new ConnectionData(Array.Empty<int>());

    public int[] Connections;
    public ConnectionData(int[] connections)
    {
        Connections = connections;
    }
    public ConnectionData(XElement data)
    {
        Connections = data.ParseIntArray("Connections", ",", Array.Empty<int>());
    }
    public XElement Export(XElement data)
    {
        data.Add(new XElement("Connections", string.Join(',', Connections)));
        return data;
    }
}