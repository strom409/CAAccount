using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using BlazorDemo.DataProviders;
using BlazorDemo.Shared;

using System;

namespace BlazorDemo.Services
{


    public partial class TestService



    {
        private readonly SqlConnectionConfiguration _configuration;

        public TestService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetMessage() => "Hello from TestService!";
       
        public async Task<List<CompanyModel>> GetAllUnits()
        {
            var units = new List<CompanyModel>();
            const string query = "SELECT * FROM dbo.Unit_master";

            try
            {
                using var con = new SqlConnection(_configuration.Value);
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