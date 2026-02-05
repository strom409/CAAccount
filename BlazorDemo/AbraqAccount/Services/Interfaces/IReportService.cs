using BlazorDemo.AbraqAccount.Models;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.AbraqAccount.Services.Interfaces;

public interface IReportService
{
    Task<BalanceSheetViewModel> GetBalanceSheetAsync(DateTime asOfDate);
}

