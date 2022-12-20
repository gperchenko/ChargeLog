namespace ChargeLog.Models
{
    public class LevelState
    {
        public event Action? OnChange;
        public bool ShowForm { get; set; } = false;
        public Dictionary<int, LevelState>? OpenItems { get; set; }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
