namespace Classes
{
    public interface ITriggerEntry
    {
        public bool isFired { get; }
        public void TryTrigger();
        public void Reset();
    }
}