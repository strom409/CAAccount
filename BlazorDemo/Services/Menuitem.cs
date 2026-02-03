// Models/MenuItem.cs
using System.Collections.Generic;

public class MenuItem
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public int? ParentId { get; set; }
    public int Order { get; set; }
    public string CssClass { get; set; }
    public List<MenuItem> Children { get; set; } = new();

    // Add these two properties
    public bool IsExpanded { get; set; }
    public bool IsActive { get; set; }
}