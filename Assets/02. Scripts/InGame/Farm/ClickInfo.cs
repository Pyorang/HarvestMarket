public struct ClickInfo
{
    public ResourceType ResourceType;
    public double Amount;

    public ClickInfo(ResourceType resourceType, double amount)
    {
        ResourceType = resourceType;
        Amount = amount;
    }
}
