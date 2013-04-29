namespace GeocachingPlus.Model.Api.GeocachingCom
{
public sealed class ImmutablePair<L, R> {
    public readonly L left;
    public readonly R right;
    
    public ImmutablePair(L left, R right)
    {
        this.left = left;
        this.right = right;
    }
}}
