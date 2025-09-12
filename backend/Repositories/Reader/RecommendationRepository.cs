using backend.DTOs;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace backend.Repositories.RecommendationRepository
{
    public class RecommendationRepository
    {
        /**
 * 数据库连接字符串
 */
        private readonly string _connectionString;

        /**
         * 构造函数
         * @param connectionString 数据库连接字符串
         * @return 无
         */
        public RecommendationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 获取某个读者的推荐书籍
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="topN">推荐书籍数量</param>
        /// <returns>推荐书籍列表</returns>
        public async Task<IEnumerable<RecommendedBookDto>> GetRecommendationsAsync(long readerId, int topN = 10)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new OracleCommand("get_recommendations", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // ====== 函数返回值（必须第一个参数） ======
            var returnCursor = new OracleParameter
            {
                ParameterName = "RETURN_VALUE",   // 固定写法
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(returnCursor);

            // ====== 输入参数 ======
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32).Value = readerId;
            cmd.Parameters.Add("p_TopN", OracleDbType.Int32).Value = topN;

            // ====== 执行函数，读取返回的 RefCursor ======
            using var reader = await cmd.ExecuteReaderAsync();
            var result = new List<RecommendedBookDto>();

            while (await reader.ReadAsync())
            {
                result.Add(new RecommendedBookDto
                {
                    ISBN = reader["ISBN"]?.ToString(),
                    Title = reader["Title"]?.ToString(),
                    Author = reader["Author"]?.ToString(),
                    BooklistCount = Convert.ToInt32(reader["BooklistCount"])
                });
            }

            return result;
        }

    }

}

