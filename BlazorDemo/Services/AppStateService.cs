using System;

public class AppStateService
{
    public event Action OnChange;

    public void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}