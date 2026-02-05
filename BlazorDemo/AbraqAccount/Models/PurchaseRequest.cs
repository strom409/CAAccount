using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.AbraqAccount.Models;

public class PurchaseRequest
{
    public int Id { get; set; }
    
    public string PORequestNo { get; set; } = string.Empty; // Auto-generated
    
    [Required]
    public DateTime RequestDate { get; set; } = DateTime.Now;
    
    [Required]
    public string RequestedBy { get; set; } = string.Empty; // Username
    
    [Required]
    public string AssignedTo { get; set; } = string.Empty; // Username
    
    [Required]
    public string RequestType { get; set; } = string.Empty;
    
    public string? Remarks { get; set; }
    
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, etc.
    
    public string? TermsAndConditions { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public List<PurchaseRequestItem> Items { get; set; } = new List<PurchaseRequestItem>();
}

public class PurchaseRequestItem
{
    public int Id { get; set; }
    public int PurchaseRequestId { get; set; }
    
    [Required]
    public string ItemName { get; set; } = string.Empty;
    
    [Required]
    public string UOM { get; set; } = string.Empty;
    
    public string? ItemDescription { get; set; }
    
    [Required]
    public decimal Qty { get; set; }
    
    [Required]
    public string UseOfItem { get; set; } = string.Empty;
    
    public string? ItemRemarks { get; set; }
    
    public bool IsReturnable { get; set; }
    
    public bool IsReusable { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation property
    public PurchaseRequest? PurchaseRequest { get; set; }
}


