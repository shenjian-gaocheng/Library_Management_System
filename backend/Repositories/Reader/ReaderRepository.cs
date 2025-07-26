using Dapper;
using Oracle.ManagedDataAccess.Client;


using backend.Models;


namespace backend.Repositories.ReaderRepository;

public class ReaderRepository
{
    /**
     * 数据库连接字符串
     * 
     */
    private readonly string _connectionString;

    /**
     * 构造函数
     * @param connectionString 数据库连接字符串
     * @return 无
     */
    public ReaderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /**
     * 根据 ReaderID 获取 Reader 对象
     * @param connectionString 数据库连接字符串
     * @return 返回一个 Reader 对象
     */
    public async Task<Reader> GetByIDAsync(string readerID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM Reader WHERE ReaderID = :ReaderID";
        return await connection.QueryFirstOrDefaultAsync<Reader>(sql, new { ReaderID = readerID });
    }

    /**
     * 根据 UserName 获取 Reader 对象
     * @param connectionString 数据库连接字符串
     * @return 返回一个 Reader 对象
     */
    public async Task<Reader> GetByUserNameAsync(string userName)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM Reader WHERE UserName = :UserName";
        return await connection.QueryFirstOrDefaultAsync<Reader>(sql, new { UserName = userName});
    }

    /**
     * 获取所有 Reader 对象
     * @return 返回一个 Reader 对象列表
     */
    public async Task<IEnumerable<Reader>> GetAllAsync()
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM Reader";
        return await connection.QueryAsync<Reader>(sql);
    }

    /**
     * 添加一个 Reader 对象
     * @param reader Reader 对象
     * @return 返回受影响的行数
     */
    public async Task<int> AddAsync(Reader reader)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            INSERT INTO Reader (ReaderID, Password, Name, CreditScore, ReaderType, AccountStatus, Permission)
            VALUES (:ReaderID, :Password, :Name, :CreditScore, :ReaderType, :AccountStatus, :Permission)";

        return await connection.ExecuteAsync(sql, reader);
    }

    /**
     * 更新一个 Reader 对象
     * @param reader Reader 对象
     * @return 返回受影响的行数
     */
    public async Task<int> UpdateAsync(Reader reader)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            UPDATE Reader
            SET Password = :Password,
                Name = :Name,
                CreditScore = :CreditScore,
                ReaderType = :ReaderType,
                AccountStatus = :AccountStatus,
                Permission = :Permission
            WHERE ReaderID = :ReaderID";

        return await connection.ExecuteAsync(sql, reader);
    }

    /**
     * 根据 ReaderID 删除一个 Reader 对象
     * @param readerID ReaderID
     * @return 返回受影响的行数
     */
    public async Task<int> DeleteAsync(string readerID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "DELETE FROM Reader WHERE ReaderID = :ReaderID";
        return await connection.ExecuteAsync(sql, new { ReaderID = readerID });
    }
}
