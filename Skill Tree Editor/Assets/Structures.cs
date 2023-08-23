using System.Xml.Linq;
public readonly struct NodeData
{
    public static NodeData Empty = new();

    public readonly string Title;
    public readonly string Description;

    //What reward type
    public readonly NodeReward Reward;
    //What reward inside of the type
    public readonly int RewardIndex;
    //amount of that reward
    public readonly int RewardAmount;
    public readonly bool IsPercentage;
    public NodeData(string title, string description, NodeReward reward, int rewardIndex, int rewardAmount, bool isPercentage)
    {
        Title = title;
        Description = description;
        Reward = reward;
        RewardIndex = rewardIndex;
        RewardAmount = rewardAmount;
        IsPercentage = isPercentage;
    }
    public NodeData(XElement data)
    {
        Title = data.ParseString("DisplayTitle", string.Empty);
        Description = data.ParseString("Description", string.Empty);
        Reward = (NodeReward)data.ParseInt("NodeReward");
        RewardIndex = data.ParseInt("RewardIndex");
        RewardAmount = data.ParseInt("RewardAmount");
        IsPercentage = data.ParseBool("IsPercentage");
    }
    public XElement Export(XElement data)
    {
        data.Add(new XElement("DisplayTitle", Title));
        data.Add(new XElement("Description", Description));
        data.Add(new XElement("NodeReward", (int)Reward));
        data.Add(new XElement("RewardIndex", RewardIndex));
        data.Add(new XElement("RewardAmount", RewardAmount));
        data.Add(new XElement("IsPercentage", IsPercentage));
        return data;
    }
}