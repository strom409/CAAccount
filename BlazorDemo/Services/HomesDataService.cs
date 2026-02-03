using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using BlazorDemo.DataProviders;
using BlazorDemo.Services;
using System;

namespace BlazorDemo.Services
{
    public partial class HomesDataService
    {
        private readonly IHomesDataProvider _dataProvider;
        //private readonly SqlConnectionConfiguration _configuration;

        public HomesDataService(IHomesDataProvider dataProvider, SqlConnectionConfiguration configuration)
        {
            _dataProvider = dataProvider;
           // _configuration = configuration;
        }
        private const string newcon = "Server=MININT-6GEVEKH\\SQL19;Database=cadb2025;user=SA;password=123;TrustServerCertificate=True;MultipleActiveResultSets=true";

        public async Task<List<CompanyModel>> GetAllUnits()
        {
            var units = new List<CompanyModel>();
            const string query = "SELECT * FROM dbo.Unit_master";

            try
            {
                using var con = new SqlConnection(newcon);
                using var cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                await con.OpenAsync();

                using var rdr = await cmd.ExecuteReaderAsync();

                while (await rdr.ReadAsync())
                {
                    var companyModel = new CompanyModel
                    {
                        Uid = rdr.GetInt32(rdr.GetOrdinal("id")),
                        Unitname = rdr["UnitName"]?.ToString() ?? string.Empty,
                        Ucode = rdr["Ucode"]?.ToString() ?? string.Empty,
                        Ustat = rdr["Stat"]?.ToString() ?? string.Empty,
                        Unitdetails = rdr["details"]?.ToString() ?? string.Empty,
                    };
                    units.Add(companyModel);
                }
            }
            catch (Exception ex)
            {
                // Log or write to console for debugging
                Console.Error.WriteLine($"Error in GetAllUnits: {ex}");
                throw;  // Rethrow to propagate error to caller
            }

            return units;
        }
    }
}