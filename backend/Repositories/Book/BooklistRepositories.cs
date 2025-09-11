using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs.Book;
using Dapper;
using Oracle.ManagedDataAccess.Types;

using Dapper.Oracle;                     // 关键：支持 OracleDynamicParameters
using Dapper.Oracle;                  // 用于 OracleDynamicParameters、OracleMappingType

namespace Backend.Repositories.Book
{
    public class BooklistRepository : IBooklistRepository
    {
        private readonly IOracleConnectionFactory _connectionFactory;

        public BooklistRepository(IOracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<BooklistSuccessResponse> AddBookToBooklistAsync(int booklistId, AddBookToBooklistRequest request)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ISBN", request.ISBN);
            p.Add("p_Notes", request.Notes);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("AddBookToBooklist", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }

        public async Task<BooklistSuccessResponse> RemoveBookFromBooklistAsync(int booklistId, string isbn)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ISBN", isbn);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("RemoveBookFromBooklist", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }

        public async Task<BooklistSuccessResponse> CollectBooklistAsync(int booklistId, int readerId, CollectBooklistRequest request)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ReaderID", readerId);
            p.Add("p_Notes", request.Notes);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("CollectBooklist", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }

        public async Task<BooklistSuccessResponse> CancelCollectBooklistAsync(int booklistId, int readerId)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ReaderID", readerId);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("CancelCollectBooklist", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }

        public async Task<BooklistSuccessResponse> UpdateCollectNotesAsync(int booklistId, int readerId, UpdateCollectNotesRequest request)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ReaderID", readerId);
            p.Add("p_NewNotes", request.NewNotes);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("UpdateCollectNotes", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }

        public async Task<CreateBooklistResponse> CreateBooklistAsync(CreateBooklistRequest request, int creatorId)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistName", request.BooklistName);
            p.Add("p_BooklistIntroduction", request.BooklistIntroduction);
            p.Add("p_CreatorID", creatorId);
            p.Add("p_BooklistID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("CreateBooklist", p, commandType: CommandType.StoredProcedure);
            return new CreateBooklistResponse { BooklistId = p.Get<int>("p_BooklistID"), Success = p.Get<int>("p_Success") };
        }

        public async Task<BooklistSuccessResponse> DeleteBooklistAsync(int booklistId, int readerId)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ReaderID", readerId);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("DeleteBooklist", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }

        public async Task<GetBooklistDetailsResponse?> GetBooklistDetailsAsync(int booklistId) 
        {
            using var conn = await _connectionFactory.CreateAsync();

            // 用 OracleDynamicParameters 来处理 RefCursor
            var p = new OracleDynamicParameters();
            p.Add("p_BooklistID", booklistId, OracleMappingType.Int32, ParameterDirection.Input);
            p.Add("p_BooklistInfo", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            p.Add("p_BooksInfo", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            using var multi = await conn.QueryMultipleAsync(
                "GetBooklistDetails",
                param: p,
                commandType: CommandType.StoredProcedure);

            // 先读书单信息
            var info = await multi.ReadFirstOrDefaultAsync<BooklistInfoDto>();
            if (info == null) return null;

            // 再读书籍列表
            var books = (await multi.ReadAsync<BookItemDto>()).AsList();

            return new GetBooklistDetailsResponse
            {
                BooklistInfo = info,
                Books = books
            };
        }

        public async Task<RecommendBooklistsResponse> RecommendBooklistsAsync(int booklistId, int limit = 10)
        {
            using var conn = await _connectionFactory.CreateAsync();

            var p = new OracleDynamicParameters();
            p.Add("p_BooklistID", booklistId, OracleMappingType.Int32, ParameterDirection.Input);
            p.Add("p_Limit", limit, OracleMappingType.Int32, ParameterDirection.Input);
            p.Add("p_RecommendedBooklists", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            var result = await conn.QueryAsync<RecommendBooklistDto>(
                "RecommendBooklists",
                param: p,
                commandType: CommandType.StoredProcedure);

            return new RecommendBooklistsResponse
            {
                Items = result?.AsList() ?? new List<RecommendBooklistDto>()
            };
        }

        public async Task<SearchBooklistsByReaderResponse> SearchBooklistsByReaderAsync(int readerId)
        {
            using var conn = await _connectionFactory.CreateAsync();

            // 用 OracleDynamicParameters 来处理 RefCursor
            var p = new OracleDynamicParameters();

            p.Add("p_ReaderID", readerId, OracleMappingType.Int32, ParameterDirection.Input);
            p.Add("p_CreatedBooklists", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            p.Add("p_CollectedBooklists", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            // 直接用 QueryMultipleAsync 获取两个结果集
            using var multi = await conn.QueryMultipleAsync(
                "SearchBooklistsByReader",
                param: p,
                commandType: CommandType.StoredProcedure);

            // 先读 Created
            var created = (await multi.ReadAsync<SimpleBooklistDto>()).AsList();
            // 再读 Collected
            var collected = (await multi.ReadAsync<SimpleBooklistDto>()).AsList();

            return new SearchBooklistsByReaderResponse
            {
                Created = created,
                Collected = collected
            };
        }


        public async Task<BooklistSuccessResponse> UpdateBooklistNameAsync(int booklistId, int readerId, UpdateBooklistNameRequest request)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ReaderID", readerId);
            p.Add("p_NewName", request.NewName);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("UpdateBooklistName", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }

        public async Task<BooklistSuccessResponse> UpdateBooklistIntroAsync(int booklistId, int readerId, UpdateBooklistIntroRequest request)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var p = new DynamicParameters();
            p.Add("p_BooklistID", booklistId);
            p.Add("p_ReaderID", readerId);
            p.Add("p_NewIntro", request.NewIntro);
            p.Add("p_Success", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("UpdateBooklistIntro", p, commandType: CommandType.StoredProcedure);
            return new BooklistSuccessResponse { Success = p.Get<int>("p_Success") };
        }
    }
}
