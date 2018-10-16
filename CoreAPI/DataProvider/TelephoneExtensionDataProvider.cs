using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using CoreAPI.Models;
using System.Data;
using System;
using System.Collections;

namespace CoreAPI.DataProvider
{
    public class TelephoneExtensionDataProvider : ITelephoneExtensionDataProvider
    {
        private readonly string connectionString = "Server=192.168.212.205\\BIOAPPDEV01;Initial Catalog=BIOFARMA;Persist Security Info=True;User ID=sysdev;Password=P@ssw0rd123";
        //private SqlConnection sqlConnection;

        public Task AddExtension(TelephoneExtension TelephoneExtension)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteExtension(int RecordId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TelephoneExtension> GetExtension(int RecordId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pRecordId", RecordId);
                IEnumerable extension = sqlConnection.QuerySingleOrDefault("bioumum.usp_GetTelephoneExtension", dynamicParameters, commandType: CommandType.StoredProcedure);
                var castItem = (IDictionary<string, object>)extension;
                var result = new TelephoneExtension
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
                            ExtensionType = castItem["ETYPE"].ToString(),
                            ExtensionName = castItem["EXTNM"].ToString(),
                            Number = castItem["NUMBR"].ToString(),
                            ChangeDate = Convert.ToDateTime(castItem["CHGDT"]),
                            UserChanger = castItem["CHUSR"].ToString()
                        });
                    }
                    IEnumerable<TelephoneExtension> en = lsTelephoneExtensions;
                    return en;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    return sqlConnection.Query<TelephoneExtension>("[bioumum].[usp_GetTelephoneExtension]", null, commandType: CommandType.StoredProcedure);
                }
            }
            
        }

        public Task UpdateExtension(TelephoneExtension TelephoneExtension)
        {
            throw new System.NotImplementedException();
        }
    }
}
