using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs.Book;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Backend.Repositories.Book
{
    public class BooklistRepository : IBooklistRepository
    {
        private readonly IOracleConnectionFactory _factory;
        public BooklistRepository(IOracleConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<CreateBooklistResponse> CreateBooklistAsync(string name, string? intro, int creatorId)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("CreateBooklist", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_BooklistName", OracleDbType.Varchar2, name, ParameterDirection.Input);
            cmd.Parameters.Add("p_BooklistIntroduction", OracleDbType.Clob, (object?)intro ?? DBNull.Value, ParameterDirection.Input);
            cmd.Parameters.Add("p_CreatorID", OracleDbType.Int32, creatorId, ParameterDirection.Input);

            var outId = new OracleParameter("p_BooklistID", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outId);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);

            await cmd.ExecuteNonQueryAsync();

            return new CreateBooklistResponse
            {
                BooklistId = Convert.ToInt32(outId.Value?.ToString() ?? "0"),
                Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0")
            };
        }

        public async Task<BooklistSuccessResponse> DeleteBooklistAsync(int booklistId, int readerId)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("DeleteBooklist", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32, readerId, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }

        public async Task<BooklistSuccessResponse> AddBookToBooklistAsync(int booklistId, string isbn, string? notes)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("AddBookToBooklist", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ISBN", OracleDbType.Varchar2, isbn, ParameterDirection.Input);
            cmd.Parameters.Add("p_Notes", OracleDbType.Clob, (object?)notes ?? DBNull.Value, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }

        public async Task<BooklistSuccessResponse> RemoveBookFromBooklistAsync(int booklistId, string isbn)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("RemoveBookFromBooklist", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ISBN", OracleDbType.Varchar2, isbn, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }

        public async Task<BooklistSuccessResponse> CollectBooklistAsync(int booklistId, int readerId, string? notes)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("CollectBooklist", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32, readerId, ParameterDirection.Input);
            cmd.Parameters.Add("p_Notes", OracleDbType.Clob, (object?)notes ?? DBNull.Value, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }

        public async Task<BooklistSuccessResponse> CancelCollectBooklistAsync(int booklistId, int readerId)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("CancelCollectBooklist", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32, readerId, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }

        public async Task<GetBooklistDetailsResponse> GetBooklistDetailsAsync(int booklistId)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("GetBooklistDetails", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);

            var cur1 = new OracleParameter("p_BooklistInfo", OracleDbType.RefCursor, ParameterDirection.Output);
            var cur2 = new OracleParameter("p_BooksInfo", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add(cur1);
            cmd.Parameters.Add(cur2);

            await cmd.ExecuteNonQueryAsync();

            var result = new GetBooklistDetailsResponse();
            
            // 处理第一个游标
            using (var reader1 = ((OracleRefCursor)cur1.Value).GetDataReader())
            {
                if (await reader1.ReadAsync())
                {
                    result.BooklistInfo = new BooklistInfoDto
                    {
                        BooklistId = reader1.GetInt32(reader1.GetOrdinal("BooklistID")),
                        ListCode = reader1.GetString(reader1.GetOrdinal("ListCode")),
                        BooklistName = reader1.GetString(reader1.GetOrdinal("BooklistName")),
                        BooklistIntroduction = reader1.IsDBNull(reader1.GetOrdinal("BooklistIntroduction")) ? null : reader1.GetString(reader1.GetOrdinal("BooklistIntroduction")),
                        CreatorId = reader1.GetInt32(reader1.GetOrdinal("CreatorID")),
                        CreatorUsername = reader1.GetString(reader1.GetOrdinal("CreatorUsername")),
                        CreatorNickname = reader1.GetString(reader1.GetOrdinal("CreatorNickname"))
                    };
                }
            }

            // 处理第二个游标
            using (var reader2 = ((OracleRefCursor)cur2.Value).GetDataReader())
            {
                while (await reader2.ReadAsync())
                {
                    result.Books.Add(new BookItemDto
                    {
                        ISBN = reader2.GetString(reader2.GetOrdinal("ISBN")),
                        AddTime = reader2.GetDateTime(reader2.GetOrdinal("AddTime")),
                        Notes = reader2.IsDBNull(reader2.GetOrdinal("Notes")) ? null : reader2.GetString(reader2.GetOrdinal("Notes")),
                        Title = reader2.GetString(reader2.GetOrdinal("Title")),
                        Author = reader2.GetString(reader2.GetOrdinal("Author")),
                    });
                }
            }
            return result;
        }

        public async Task<RecommendBooklistsResponse> RecommendBooklistsAsync(int booklistId, int limit = 10)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("RecommendBooklists", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_Limit", OracleDbType.Int32, limit, ParameterDirection.Input);
            var cur = new OracleParameter("p_RecommendedBooklists", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add(cur);

            await cmd.ExecuteNonQueryAsync();

            var resp = new RecommendBooklistsResponse();
            using var reader = ((OracleRefCursor)cur.Value).GetDataReader();
            while (await reader.ReadAsync())
            {
                resp.Items.Add(new RecommendBooklistDto
                {
                    BooklistId = reader.GetInt32(reader.GetOrdinal("BooklistID")),
                    ListCode = reader.GetString(reader.GetOrdinal("ListCode")),
                    BooklistName = reader.GetString(reader.GetOrdinal("BooklistName")),
                    BooklistIntroduction = reader.IsDBNull(reader.GetOrdinal("BooklistIntroduction")) ? null : reader.GetString(reader.GetOrdinal("BooklistIntroduction")),
                    CreatorId = reader.GetInt32(reader.GetOrdinal("CreatorID")),
                    CreatorNickname = reader.GetString(reader.GetOrdinal("CreatorNickname")),
                    MatchingBooksCount = reader.GetInt32(reader.GetOrdinal("MatchingBooksCount")),
                    SimilarityScore = reader.GetDecimal(reader.GetOrdinal("SimilarityScore"))
                });
            }
            return resp;
        }

        public async Task<SearchBooklistsByReaderResponse> SearchBooklistsByReaderAsync(int readerId)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("SearchBooklistsByReader", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32, readerId, ParameterDirection.Input);
            var curCreated = new OracleParameter("p_CreatedBooklists", OracleDbType.RefCursor, ParameterDirection.Output);
            var curCollected = new OracleParameter("p_CollectedBooklists", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add(curCreated);
            cmd.Parameters.Add(curCollected);

            await cmd.ExecuteNonQueryAsync();

            var resp = new SearchBooklistsByReaderResponse();
            
            // 处理创建的书单
            using (var r1 = ((OracleRefCursor)curCreated.Value).GetDataReader())
            {
                while (await r1.ReadAsync())
                {
                    resp.Created.Add(new SimpleBooklistDto
                    {
                        BooklistId = r1.GetInt32(r1.GetOrdinal("BooklistID")),
                        ListCode = r1.GetString(r1.GetOrdinal("ListCode")),
                        BooklistName = r1.GetString(r1.GetOrdinal("BooklistName")),
                        BooklistIntroduction = r1.IsDBNull(r1.GetOrdinal("BooklistIntroduction")) ? null : r1.GetString(r1.GetOrdinal("BooklistIntroduction")),
                        CreatorId = r1.GetInt32(r1.GetOrdinal("CreatorID")),
                        FavoriteTime = null
                    });
                }
            }

            // 处理收藏的书单
            using (var r2 = ((OracleRefCursor)curCollected.Value).GetDataReader())
            {
                while (await r2.ReadAsync())
                {
                    resp.Collected.Add(new SimpleBooklistDto
                    {
                        BooklistId = r2.GetInt32(r2.GetOrdinal("BooklistID")),
                        ListCode = r2.GetString(r2.GetOrdinal("ListCode")),
                        BooklistName = r2.GetString(r2.GetOrdinal("BooklistName")),
                        BooklistIntroduction = r2.IsDBNull(r2.GetOrdinal("BooklistIntroduction")) ? null : r2.GetString(r2.GetOrdinal("BooklistIntroduction")),
                        CreatorId = r2.GetInt32(r2.GetOrdinal("CreatorID")),
                        FavoriteTime = r2.GetDateTime(r2.GetOrdinal("FavoriteTime"))
                    });
                }
            }

            return resp;
        }

        public async Task<BooklistSuccessResponse> UpdateBooklistNameAsync(int booklistId, int readerId, string newName)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("UpdateBooklistName", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32, readerId, ParameterDirection.Input);
            cmd.Parameters.Add("p_NewName", OracleDbType.Varchar2, newName, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }

        public async Task<BooklistSuccessResponse> UpdateBooklistIntroAsync(int booklistId, int readerId, string newIntro)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("UpdateBooklistIntro", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32, readerId, ParameterDirection.Input);
            cmd.Parameters.Add("p_NewIntro", OracleDbType.Clob, (object?)newIntro ?? DBNull.Value, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }

        public async Task<BooklistSuccessResponse> UpdateCollectNotesAsync(int booklistId, int readerId, string newNotes)
        {
            using var conn = await _factory.CreateAsync();
            using var cmd = new OracleCommand("UpdateCollectNotes", (OracleConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("p_BooklistID", OracleDbType.Int32, booklistId, ParameterDirection.Input);
            cmd.Parameters.Add("p_ReaderID", OracleDbType.Int32, readerId, ParameterDirection.Input);
            cmd.Parameters.Add("p_NewNotes", OracleDbType.Clob, (object?)newNotes ?? DBNull.Value, ParameterDirection.Input);
            var outSuccess = new OracleParameter("p_Success", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outSuccess);
            await cmd.ExecuteNonQueryAsync();
            return new BooklistSuccessResponse { Success = Convert.ToInt32(outSuccess.Value?.ToString() ?? "0") };
        }
    }
}