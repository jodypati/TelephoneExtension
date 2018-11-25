using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using CoreAPI.Models;
using System.Data;
using System;
using System.Collections;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CoreAPI.Helpers;

namespace CoreAPI.DataProvider
{
    public class TelephoneExtensionDataProvider : ITelephoneExtensionDataProvider
    {
        //private ITelephoneExtensionDataProvider telephoneExtensionDataProvider;
        public static string GetBioFarmaConnectionString()
        {
            return Startup.BiofarmaConnectionString;
        }
        private readonly string connectionString = GetBioFarmaConnectionString(); //"Server=192.168.212.205\\BIOAPPDEV01;Initial Catalog=BIOFARMA;Persist Security Info=True;User ID=sysdev;Password=P@ssw0rd123";
        //private SqlConnection sqlConnection;

        public async Task<int> AddExtension(TelephoneExtension TelephoneExtension)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pRECID", TelephoneExtension.RecordId);
                dynamicParameters.Add("@pPRTID", TelephoneExtension.ParentId);
                dynamicParameters.Add("@pETYPE", TelephoneExtension.ExtensionType);
                dynamicParameters.Add("@pEXTNM", TelephoneExtension.ExtensionName);
                dynamicParameters.Add("@pNUMBR", TelephoneExtension.Number);
                dynamicParameters.Add("@pCHUSR", Startup.userNumber);
                dynamicParameters.Add("@pActio", 0);

                var id = await sqlConnection.ExecuteScalarAsync<int>(
                    "bioumum.usp_MaintainTelephoneExtension",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task DeleteExtension(int RecordId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pRECID", RecordId);
                dynamicParameters.Add("@pCHUSR", Startup.userNumber);
                dynamicParameters.Add("@pActio", 3);

                await sqlConnection.ExecuteAsync(
                    "bioumum.usp_MaintainTelephoneExtension",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
                //GetExtension(TelephoneExtension.RecordId);

            }
        }

        public async Task<TelephoneExtension> GetExtension(int RecordId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pRecordId", RecordId);
                IEnumerable extension = sqlConnection.QuerySingleOrDefault("bioumum.usp_GetTelephoneExtension", dynamicParameters, commandType: CommandType.StoredProcedure);
                TelephoneExtension result = new TelephoneExtension();
                if (extension != null)
                {
                    var castItem = (IDictionary<string, object>)extension;
                    result = new TelephoneExtension
                    {
                        BeginDate = Convert.ToDateTime(castItem["BEGDA"]),
                        EndDate = Convert.ToDateTime(castItem["ENDDA"]),
                        RecordId = Convert.ToInt32(castItem["RECID"]),
                        ExtensionType = castItem["ETYPE"].ToString(),
                        ExtensionName = castItem["EXTNM"].ToString(),
                        Number = castItem["NUMBR"].ToString(),
                        ChangeDate = Convert.ToDateTime(castItem["CHGDT"]),
                        UserChanger = castItem["CHUSR"].ToString()
                    };
                    return result;
                }

                return null;
            }
        }

        public async Task<IEnumerable<TelephoneExtension>> GetExtensions()
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();

                    IEnumerable extension = sqlConnection.Query("bioumum.usp_GetTelephoneExtension", null, commandType: CommandType.StoredProcedure);
                    List<TelephoneExtension> lsTelephoneExtensions = new List<TelephoneExtension>();
                    foreach(var item in extension)
                    {
                        var castItem = (IDictionary<string, object>)item;
                        lsTelephoneExtensions.Add(new TelephoneExtension
                        {
                            BeginDate = Convert.ToDateTime(castItem["BEGDA"]),
                            EndDate = Convert.ToDateTime(castItem["ENDDA"]),
                            RecordId = Convert.ToInt32(castItem["RECID"]),
                            ExtensionType = Convert.ToString(castItem["ETYPE"]),
                            ExtensionName = Convert.ToString(castItem["EXTNM"]),
                            Number = Convert.ToString(castItem["NUMBR"]),
                            ChangeDate = Convert.ToDateTime(castItem["CHGDT"]),
                            UserChanger = Convert.ToString(castItem["CHUSR"])
                        });

                    }

                    //IEnumerable<TelephoneExtension> en;// = lsTelephoneExtensions;
                    return lsTelephoneExtensions;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    return null;//sqlConnection.Query<TelephoneExtension>("[bioumum].[usp_GetTelephoneExtension]", null, commandType: CommandType.StoredProcedure);
                }
            }
            
        }

        public async Task UpdateExtension(TelephoneExtension TelephoneExtension)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pRECID", TelephoneExtension.RecordId);
                dynamicParameters.Add("@pPRTID", TelephoneExtension.ParentId);
                dynamicParameters.Add("@pETYPE", TelephoneExtension.ExtensionType);
                dynamicParameters.Add("@pEXTNM", TelephoneExtension.ExtensionName);
                dynamicParameters.Add("@pNUMBR", TelephoneExtension.Number);
                dynamicParameters.Add("@pCHUSR", Startup.userNumber);
                dynamicParameters.Add("@pActio", 1);

                await sqlConnection.ExecuteAsync(
                    "bioumum.usp_MaintainTelephoneExtension",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
                //GetExtension(TelephoneExtension.RecordId);
                
            }
        }
    }
}
