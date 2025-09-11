public class CommentRepository
{
    private readonly string _connectionString;

    public CommentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<CommentDetailDto>> SearchCommentAsync(string ISBN)
    {
        var sql = @"
            SELECT *
            FROM COMMENT_TABLE
            WHERE ISBN = :ISBN and status = '正常'";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryAsync<CommentDetailDto>(connection, sql, new { ISBN = ISBN });
    }

    
    public async Task<IEnumerable<CommentDetailDto>> SearchCommentAsyncByCommentID(decimal comment_id)
    {
        var sql = @"
            SELECT *
            FROM COMMENT_TABLE
            WHERE commentID = :comment_id";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        Console.WriteLine($"id = {comment_id}");
        return await Dapper.SqlMapper.QueryAsync<CommentDetailDto>(connection, sql, new { comment_id = comment_id });
    }
    
    public async Task<int> AddCommentAsync(CommentDetailDto commentDto)
    {
        var sql = @"
            INSERT INTO Comment_Table (READERID, ISBN, RATING, REVIEWCONTENT, CREATETIME, STATUS)
            VALUES (:ReaderID, :ISBN, :Rating, :REVIEWCONTENT, :CREATETIME, :Status)";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();
        
        return await Dapper.SqlMapper.ExecuteAsync(connection, sql, commentDto);
    }
}

