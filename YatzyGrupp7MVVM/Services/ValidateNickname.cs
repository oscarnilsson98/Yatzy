using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using System.Data;
using YatzyGrupp7MVVM.Models;
using YatzyGrupp7MVVM.Data;

namespace YatzyGrupp7MVVM.Services
{
    public interface IService
    {
        bool ValidateNickName(string nickname, out ICollection<string> validationErrors);
    }

    public class ValidateNickname : IService
    {
        public bool ValidateNickName(string nickname, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();
            int count = 0;
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
            {
                var output = connection.QuerySingle<int>($@"SELECT COUNT(*) FROM player WHERE Nickname = @Nickname", new { Nickname = nickname });

                count = output;
                
            }
            if (count > 0)
                validationErrors.Add("Användarnamnet finns redan, välj ett annat");

            return validationErrors.Count == 0;
        }
    }
}
