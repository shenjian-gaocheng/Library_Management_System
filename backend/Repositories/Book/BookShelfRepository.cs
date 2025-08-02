public class BookShelfRepository
{
    private readonly string _connectionString;

    public BookShelfRepository(string connectionString)
    {
        _connectionString = connectionString;
    }



    public async Task<IEnumerable<BookDto>> SearchBookWhichShelfAsync(string keyword)
    {
        var sql = @"
        SELECT b.SHELFID,  
               i.TITLE 
        FROM Book b
         JOIN BookInfo i ON b.ISBN = i.ISBN
        WHERE LOWER(i.TITLE) LIKE :keyword";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryAsync<BookDto>(
            connection, sql, new { keyword = $"%{keyword.ToLower()}%" });
    }
}