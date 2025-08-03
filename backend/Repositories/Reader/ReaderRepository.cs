using Dapper;
using Oracle.ManagedDataAccess.Client;


using backend.Models;
using backend.Common.Utils;


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
    public async Task<Reader> GetByReaderIDAsync(long readerID)
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
        var sql = "SELECT * FROM Reader WHERE Username = :UserName";
        return await connection.QueryFirstOrDefaultAsync<Reader>(sql, new { UserName = userName});
    }

    /**
     * 获取所有 Reader 对象
     * @return 返回一个 Reader 对象列表
     */
    public async Task<IEnumerable<Reader>> GetAllReadersAsync()
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM Reader";
        return await connection.QueryAsync<Reader>(sql);
    }

    /**
     * 插入一个 Reader 对象
     * @param reader Reader 对象
     * @return 返回受影响的行数
     */
    public async Task<int> InsertReaderAsync(Reader reader)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        reader.Password = PasswordUtils.HashPassword(reader.Password); // 确保密码被哈希处理

        var sql = @"
            INSERT INTO Reader (Username, Password, Fullname,Nickname,Avatar, CreditScore, AccountStatus, Permission)
            VALUES (:UserName, :Password, :FullName,:NickName,:Avatar, :CreditScore,:AccountStatus, :Permission)";

        return await connection.ExecuteAsync(sql, reader);
    }

    /**
     * 更新一个 Reader 对象
     * @param reader Reader 对象
     * @return 返回受影响的行数
     */
    public async Task<int> UpdateReaderAsync(Reader reader)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        reader.Password = PasswordUtils.HashPassword(reader.Password); // 确保密码被哈希处理

        var sql = @"
            UPDATE Reader
            SET Username = :UserName,
                Password = :Password,
                Fullname = :FullName,
                Nickname = :NickName,
                Avatar = :Avatar,
                CreditScore = :CreditScore,
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
    public async Task<int> DeleteReaderAsync(long readerID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "DELETE FROM Reader WHERE ReaderID = :ReaderID";
        return await connection.ExecuteAsync(sql, new { ReaderID = readerID });
    }

    /**
     * 检查用户名是否存在
     * @param userName 用户名
     * @return 返回 true 如果存在，否则返回 false
     */
    public async Task<bool> IsUserNameExistsAsync(string userName)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT COUNT(*) FROM Reader WHERE Username = :UserName";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { UserName = userName });
        return count > 0;
    }


    /**
        * 重置密码
        */
    public async Task<int> ResetPasswordAsync(string userName, string newPassword)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            UPDATE Reader
            SET Password = :newPassword
            WHERE Username = :userName";

        return await connection.ExecuteAsync(sql, new { newPassword, userName });
    }

    /**
     * 更新头像
     */
    public async Task<int> UpdateAvatarAsync(long readerID, string newAvatar)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            UPDATE Reader
            SET Avatar = :newAvatar
            WHERE ReaderID = :readerID";

        return await connection.ExecuteAsync(sql, new { readerID, newAvatar });
    }

    /**
 * 更新一个 Reader的Profile字段 
 * @param reader Reader 对象
 * @return 返回受影响的行数
 */
    public async Task<int> UpdateProfileAsync(Reader reader)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            UPDATE Reader
            SET Username = :UserName,
                Fullname = :FullName,
                Nickname = :NickName
            WHERE ReaderID = :ReaderID";

        return await connection.ExecuteAsync(sql, reader);
    }


}
