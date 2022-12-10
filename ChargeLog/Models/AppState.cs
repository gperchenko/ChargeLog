namespace ChargeLog.Models
{
    public class AppState
    {
        public event Action? OnChange;
        public Dictionary<int, OpenItem> OpenItems = new();

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
