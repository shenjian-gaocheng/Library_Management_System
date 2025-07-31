using Dapper;
using Oracle.ManagedDataAccess.Client;
using backend.Models;

namespace backend.Repositories.BorrowRecordRepository;

public class BorrowRecordRepository
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
    public BorrowRecordRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /**
     * 根据 BorrowRecordID 获取 BorrowRecord 信息
     * @param borrowRecordID 借阅记录 ID
     * @return 返回 BorrowRecord 对象
     */
    public async Task<BorrowRecord> GetByIDAsync(int borrowRecordID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord WHERE BorrowRecordID = :BorrowRecordID";
        return await connection.QueryFirstOrDefaultAsync<BorrowRecord>(sql, new { BorrowRecordID = borrowRecordID });
    }

    /**
     * 根据 ReaderID 获取 BorrowRecord 信息
     * @param readerID 读者 ID
     * @return 返回 BorrowRecord 对象列表
     */
    public async Task<IEnumerable<BorrowRecord>> GetByReaderIDAsync(string readerID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord WHERE ReaderID = :ReaderID";
        return await connection.QueryAsync<BorrowRecord>(sql, new { ReaderID = readerID });
    }

    /**
     * 根据 BookID 获取 BorrowRecord 信息
     * @param bookID 图书 ID
     * @return 返回 BorrowRecord 对象列表
     */
    public async Task<IEnumerable<BorrowRecord>> GetByBookIDAsync(string bookID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord WHERE BookID = :BookID";
        return await connection.QueryAsync<BorrowRecord>(sql, new { BookID = bookID });
    }

    /**
     * 获取所有 BorrowRecord 信息
     * @return 返回 BorrowRecord 对象列表
     */
    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord";
        return await connection.QueryAsync<BorrowRecord>(sql);
    }

    /**
     * 新增一个 BorrowRecord
     * @param borrowRecord BorrowRecord 对象
     * @return 新增成功返回 1，否则返回 0
     */
    public async Task<int> AddAsync(BorrowRecord borrowRecord)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            INSERT INTO BorrowRecord ( ReaderID, BookID, BorrowTime, ReturnTime, OverdueFine)
            VALUES ( :ReaderID, :BookID, :BorrowTime, :ReturnTime, : OverdueFine)";

        return await connection.ExecuteAsync(sql, borrowRecord);
    }

    /**
     * 更新一个 BorrowRecord
     * @param borrowRecord BorrowRecord 对象
     * @return 更新成功返回 1，否则返回 0
     */
    public async Task<int> UpdateAsync(BorrowRecord borrowRecord)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            UPDATE BorrowRecord
            SET ReaderID = :ReaderID,
                BookID = :BookID,
                BorrowTime = :BorrowTime,
                ReturnTime = :ReturnTime,
                OverdueFine = :OverdueFine
            WHERE BorrowRecordID = :BorrowRecordID";

        return await connection.ExecuteAsync(sql, borrowRecord);
    }

    /**
     * 根据 BorrowRecordID 删除一个 BorrowRecord
     * @param borrowRecordID BorrowRecordID
     * @return 删除成功返回 1，否则返回 0
     */
    public async Task<int> DeleteAsync(string borrowRecordID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "DELETE FROM BorrowRecord WHERE BorrowRecordID = :BorrowRecordID";
        return await connection.ExecuteAsync(sql, new { BorrowRecordID = borrowRecordID });
    }
}
