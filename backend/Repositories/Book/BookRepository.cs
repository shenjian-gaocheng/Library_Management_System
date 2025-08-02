public class BookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<BookInfoDto>> SearchBooksAsync(string keyword)
    {
        var sql = @"
            SELECT ISBN, Title, Author
            FROM BookInfo
            WHERE LOWER(Title) LIKE :keyword OR LOWER(Author) LIKE :keyword";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryAsync<BookInfoDto>(
            connection, sql, new { keyword = $"%{keyword.ToLower()}%" });
    }


}