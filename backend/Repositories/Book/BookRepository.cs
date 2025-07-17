public class BookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<BookDetailDto>> SearchBooksAsync(string keyword)
    {
        var sql = @"
            SELECT BookID, ISBN, Title, Author, Status
            FROM book_detail_view
            WHERE LOWER(Title) LIKE :keyword OR LOWER(Author) LIKE :keyword";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryAsync<BookDetailDto>(
            connection, sql, new { keyword = $"%{keyword.ToLower()}%" });
    }
}