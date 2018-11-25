using CoreAPI.Helpers;
using CoreAPI.Models;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.DataProvider
{
    public interface IUserDataProvider
    {
        Task<User> Authenticate(string username, string password);
        Task<User> GetByUserNumber(string userNumber);
    }
    public class UserDataProvider : IUserDataProvider
    {
        public static string GetBioFarmaConnectionString()
        {
            return Startup.BiofarmaConnectionString;
        }
        private readonly string connectionString = GetBioFarmaConnectionString();
        
        public async Task<User> Authenticate(string username, string password)
        {
            string hostName = Environment.MachineName + "\\" + Environment.UserName;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            if (!AccessActiveDirectory.ValidateUser(username, password))
                return null;

            string nik = AccessActiveDirectory.GetUserNumberByEmail(username);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pPERNR", nik);
                IEnumerable extension = sqlConnection.QuerySingleOrDefault("bioumum.usp_GetEmployeeByPERNR", dynamicParameters, commandType: CommandType.StoredProcedure);

                User user = new User();
                if (extension != null)
                {
                    var castItem = (IDictionary<string, object>)extension;
                    user = new User
                    {
                        Name = castItem["CNAME"].ToString(),
                        UserId = castItem["PERNR"].ToString(),
                        PositionId = castItem["POSID"].ToString(),
                        UserName = username,
                        PositionName = castItem["PRPOS"].ToString(),
                        UnitCode = castItem["ORGCD"].ToString(),
                        UnitName = castItem["PRORG"].ToString(),
                        Grade = castItem["PSGRP"].ToString()
                    };
                }

                return user;
            }
        }

        public async Task<User> GetByUserNumber(string userNumber)
        {
            var nik = userNumber;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pPERNR", nik);
                IEnumerable extension = sqlConnection.QuerySingleOrDefault("bioumum.usp_GetEmployeeByPERNR", dynamicParameters, commandType: CommandType.StoredProcedure);

                User user = new User();
                if (extension != null)
                {
                    var castItem = (IDictionary<string, object>)extension;
                    user = new User
                    {
                        Name = castItem["CNAME"].ToString(),
                        UserId = castItem["PERNR"].ToString(),
                        PositionId = castItem["POSID"].ToString(),
                        PositionName = castItem["PRPOS"].ToString(),
                        UnitCode = castItem["ORGCD"].ToString(),
                        UnitName = castItem["PRORG"].ToString(),
                        Grade = castItem["PSGRP"].ToString()
                    };
                }

                return user;
            }
        }

    }
}
