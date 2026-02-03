// Models/MenuItem.cs

using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using DevExpress.Charts.Model;
using System.Runtime.Intrinsics.Arm;
using Syncfusion.Blazor.Grids;
using System.Net;
using static BlazorDemo.Pages.Definitions.EditGrowerAgreement_;
using Microsoft.VisualBasic;
using DevExpress.Charts.Native;
using Microsoft.SqlServer.Server;
using DevExpress.CodeParser;
using DevExpress.XtraRichEdit.Import.Html;
using System.Linq;
// Services/MenuService.cs
public class MenuService
{
    private readonly SqlConnectionConfiguration _configuration;

    public MenuService(SqlConnectionConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<MenuItem>> GetMenuItemsAsync()
    {
        var menuItems = new List<MenuItem>();

        using (var connection = new SqlConnection(_configuration.Value))
        {
            await connection.OpenAsync();

            // Get all menu items
            var command = new SqlCommand(
                "SELECT Id, Text, Icon, Url, ParentId, [Order], CssClass FROM MenuItems ORDER BY [Order]",
                connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    menuItems.Add(new MenuItem
                    {
                        Id = reader.GetInt32(0),
                        Text = reader.GetString(1),
                        Icon = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Url = reader.IsDBNull(3) ? null : reader.GetString(3),
                        ParentId = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4),
                        Order = reader.GetInt32(5),
                        CssClass = reader.IsDBNull(6) ? null : reader.GetString(6)
                    });
                }
            }
        }

        // Build hierarchy
        var lookup = menuItems.ToLookup(x => x.ParentId);
        foreach (var item in menuItems)
        {
            item.Children.AddRange(lookup[item.Id]);
        }

        return lookup[null].OrderBy(x => x.Order).ToList();
    }
}