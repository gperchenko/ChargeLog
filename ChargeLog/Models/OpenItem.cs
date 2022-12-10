namespace ChargeLog.Models
{
    public class OpenItem
    {
        public event Action? OnChange;
        public bool ShowForm { get; set; } = false;
        public Dictionary<int, OpenItem>? ChildItems { get; set; }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
