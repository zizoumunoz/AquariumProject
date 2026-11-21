public interface IFishBehaviour
{
    public abstract void Init(FishStats stats);
    public abstract void UpdateFish();
    public int Order { get; }
}
