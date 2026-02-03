using System;

public class DataChangeNotifier
{
    public event Action? OnChange;

    public void NotifyDataChanged()
    {
        OnChange?.Invoke();
    }
}