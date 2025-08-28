using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs.Book;
using Dapper;
using Oracle.ManagedDataAccess.Types;

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
            using var multi = await conn.QueryMultipleAsync(
                "GetBooklistDetails",
                new { p_BooklistID = booklistId },
                commandType: CommandType.StoredProcedure);

            // 先读书单信息
            var info = !multi.IsConsumed 
                ? await multi.ReadFirstOrDefaultAsync<BooklistInfoDto>() 
                : null;
            if (info == null) return null;

            // 再读书籍列表
            var books = !multi.IsConsumed 
                ? (await multi.ReadAsync<BookItemDto>()).AsList() 
                : new List<BookItemDto>();

            return new GetBooklistDetailsResponse
            {
                BooklistInfo = info,
                Books = books
            };
        }

        public async Task<RecommendBooklistsResponse> RecommendBooklistsAsync(int booklistId, int limit = 10)
        {
            using var conn = await _connectionFactory.CreateAsync();
            var result = await conn.QueryAsync<RecommendBooklistDto>(
                "RecommendBooklists",
                new { p_BooklistID = booklistId, p_Limit = limit },
                commandType: CommandType.StoredProcedure);

            return new RecommendBooklistsResponse
            {
                Items = result?.AsList() ?? new List<RecommendBooklistDto>()
            };
        }

        public async Task<SearchBooklistsByReaderResponse> SearchBooklistsByReaderAsync(int readerId)
        {
            using var conn = await _connectionFactory.CreateAsync();
            using var multi = await conn.QueryMultipleAsync(
                "SearchBooklistsByReader",
                new { p_ReaderID = readerId },
                commandType: CommandType.StoredProcedure);

            // 用户创建的书单
            var created = !multi.IsConsumed 
                ? (await multi.ReadAsync<SimpleBooklistDto>()).AsList() 
                : new List<SimpleBooklistDto>();

            // 用户收藏的书单
            var collected = !multi.IsConsumed 
                ? (await multi.ReadAsync<SimpleBooklistDto>()).AsList() 
                : new List<SimpleBooklistDto>();

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
