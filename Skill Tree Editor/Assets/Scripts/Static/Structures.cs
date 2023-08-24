using System;
using System.ComponentModel;
using System.Xml.Linq;
public readonly struct NodeData
{
    public static NodeData Empty = new();

    [DefaultValue(0)] //probably not needed
    public readonly int Type;

    public readonly string Id;

    public readonly string Title;
    public readonly string Description;
   
    //What reward type
    public readonly NodeReward Reward;
    //What reward inside of the type
    public readonly int RewardIndex;
    //amount of that reward
    public readonly int RewardAmount;
    public readonly bool IsPercentage;
    public NodeData(int type, string id, string title, string description, NodeReward reward, int rewardIndex, int rewardAmount, bool isPercentage)
    {
        Type = type;
        Id = id;
        Title = title;
        Description = description;
        Reward = reward;
        RewardIndex = rewardIndex;
        RewardAmount = rewardAmount;
        IsPercentage = isPercentage;

        if (Id == null || Id.Length == 0)
            Id = $"Skill Node {Type}";
    }
    public NodeData(XElement data)
    {
        Type = data.ParseInt("@type");
        Id = data.ParseString("@id", $"Skill Node {Type}");
        Title = data.ParseString("DisplayTitle", string.Empty);
        Description = data.ParseString("Description", string.Empty);
        Reward = (NodeReward)data.ParseInt("NodeReward");
        RewardIndex = data.ParseInt("RewardIndex");
        RewardAmount = data.ParseInt("RewardAmount");
        IsPercentage = data.ParseBool("IsPercentage");
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
        data.Add(new XElement("NodeReward", (int)Reward));
        data.Add(new XElement("RewardIndex", RewardIndex));
        data.Add(new XElement("RewardAmount", RewardAmount));
        data.Add(new XElement("IsPercentage", IsPercentage));
        return data;
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