namespace Classes
{
    public interface ITriggerEntry
    {
        bool HasFired { get; }
        void TryTrigger();
        void Reset();
    }
}