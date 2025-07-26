using Dapper;
using Oracle.ManagedDataAccess.Client;


using backend.Models;


namespace backend.Repositories.ReaderRepository;

public class ReaderRepository
{
    /**
     * ���ݿ������ַ���
     * 
     */
    private readonly string _connectionString;

    /**
     * ���캯��
     * @param connectionString ���ݿ������ַ���
     * @return ��
     */
    public ReaderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /**
     * ���� ReaderID ��ȡ Reader ����
     * @param connectionString ���ݿ������ַ���
     * @return ����һ�� Reader ����
     */
    public async Task<Reader> GetByIDAsync(string readerID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM Reader WHERE ReaderID = :ReaderID";
        return await connection.QueryFirstOrDefaultAsync<Reader>(sql, new { ReaderID = readerID });
    }

    /**
     * ���� UserName ��ȡ Reader ����
     * @param connectionString ���ݿ������ַ���
     * @return ����һ�� Reader ����
     */
    public async Task<Reader> GetByUserNameAsync(string userName)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM Reader WHERE UserName = :UserName";
        return await connection.QueryFirstOrDefaultAsync<Reader>(sql, new { UserName = userName});
    }

    /**
     * ��ȡ���� Reader ����
     * @return ����һ�� Reader �����б�
     */
    public async Task<IEnumerable<Reader>> GetAllAsync()
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM Reader";
        return await connection.QueryAsync<Reader>(sql);
    }

    /**
     * ���һ�� Reader ����
     * @param reader Reader ����
     * @return ������Ӱ�������
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
     * ����һ�� Reader ����
     * @param reader Reader ����
     * @return ������Ӱ�������
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
     * ���� ReaderID ɾ��һ�� Reader ����
     * @param readerID ReaderID
     * @return ������Ӱ�������
     */
    public async Task<int> DeleteAsync(string readerID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "DELETE FROM Reader WHERE ReaderID = :ReaderID";
        return await connection.ExecuteAsync(sql, new { ReaderID = readerID });
    }
}
