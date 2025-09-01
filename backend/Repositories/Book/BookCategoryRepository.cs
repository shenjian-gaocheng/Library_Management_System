using backend.DTOs.Book;
using backend.Models;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace backend.Repositories.Book
{
    /// <summary>
    /// 图书分类关联数据访问层
    /// </summary>
    public class BookCategoryRepository
    {
        private readonly string _connectionString;

        public BookCategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 获取连接字符串（供Service层使用）
        /// </summary>
        public string GetConnectionString()
        {
            return _connectionString;
        }

        /// <summary>
        /// 添加图书分类关联
        /// </summary>
        public async Task<int> AddBookCategoryAsync(BookCategory bookCategory)
        {
            var sql = @"
                INSERT INTO Book_Classify (ISBN, CategoryID, RelationNote)
                VALUES (:ISBN, :CategoryID, :RelationNote)";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, bookCategory);
        }

        /// <summary>
        /// 批量添加图书分类关联
        /// </summary>
        public async Task<int> AddBookCategoriesAsync(List<BookCategory> bookCategories)
        {
            if (!bookCategories.Any()) return 0;

            var sql = @"
                INSERT INTO Book_Classify (ISBN, CategoryID, RelationNote)
                VALUES (:ISBN, :CategoryID, :RelationNote)";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, bookCategories);
        }

        /// <summary>
        /// 删除图书分类关联
        /// </summary>
        public async Task<int> RemoveBookCategoryAsync(string isbn, string categoryId)
        {
            var sql = @"
                DELETE FROM Book_Classify 
                WHERE ISBN = :ISBN AND CategoryID = :CategoryID";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, new { ISBN = isbn, CategoryID = categoryId });
        }

        /// <summary>
        /// 删除图书的所有分类关联
        /// </summary>
        public async Task<int> RemoveAllBookCategoriesAsync(string isbn)
        {
            var sql = @"
                DELETE FROM Book_Classify 
                WHERE ISBN = :ISBN";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, new { ISBN = isbn });
        }

        /// <summary>
        /// 检查图书分类关联是否存在
        /// </summary>
        public async Task<bool> ExistsBookCategoryAsync(string isbn, string categoryId)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM Book_Classify
                WHERE ISBN = :ISBN AND CategoryID = :CategoryID";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var count = await connection.ExecuteScalarAsync<int>(sql, new { ISBN = isbn, CategoryID = categoryId });
            return count > 0;
        }

        /// <summary>
        /// 获取图书的所有分类关联
        /// </summary>
        public async Task<IEnumerable<BookCategoryDetailDto>> GetBookCategoriesAsync(string isbn)
        {
            var sql = @"
                SELECT 
                    bc.ISBN,
                    bi.Title,
                    bi.Author,
                    bc.CategoryID,
                    c.CategoryName,
                    bc.RelationNote
                FROM Book_Classify bc
                JOIN BookInfo bi ON bc.ISBN = bi.ISBN
                JOIN Category c ON bc.CategoryID = c.CategoryID
                WHERE bc.ISBN = :ISBN
                ORDER BY c.CategoryName";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var results = await connection.QueryAsync<BookCategoryDetailDto>(sql, new { ISBN = isbn });
            
            // 为每个结果添加分类路径
            foreach (var result in results)
            {
                result.CategoryPath = await GetCategoryPathAsync(result.CategoryID);
            }

            return results;
        }

        /// <summary>
        /// 获取分类的所有图书关联
        /// </summary>
        public async Task<IEnumerable<BookCategoryDetailDto>> GetCategoryBooksAsync(string categoryId)
        {
            var sql = @"
                SELECT 
                    bc.ISBN,
                    bi.Title,
                    bi.Author,
                    bc.CategoryID,
                    c.CategoryName,
                    bc.RelationNote
                FROM Book_Classify bc
                JOIN BookInfo bi ON bc.ISBN = bi.ISBN
                JOIN Category c ON bc.CategoryID = c.CategoryID
                WHERE bc.CategoryID = :CategoryID
                ORDER BY bi.Title";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var results = await connection.QueryAsync<BookCategoryDetailDto>(sql, new { CategoryID = categoryId });
            
            // 为每个结果添加分类路径
            foreach (var result in results)
            {
                result.CategoryPath = await GetCategoryPathAsync(result.CategoryID);
            }

            return results;
        }

        /// <summary>
        /// 获取所有叶子节点分类（用于绑定选择）
        /// </summary>
        public async Task<IEnumerable<CategorySelectDto>> GetLeafCategoriesAsync()
        {
            var sql = @"
                SELECT 
                    c.CategoryID,
                    c.CategoryName
                FROM Category c
                WHERE NOT EXISTS (
                    SELECT 1 FROM Category child 
                    WHERE child.ParentCategoryID = c.CategoryID
                )
                ORDER BY c.CategoryName";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var results = await connection.QueryAsync<CategorySelectDto>(sql);
            
            // 为每个结果添加分类路径和叶子节点标识
            foreach (var result in results)
            {
                result.CategoryPath = await GetCategoryPathAsync(result.CategoryID);
                result.IsLeaf = true;
            }

            return results;
        }

        /// <summary>
        /// 检查分类是否为叶子节点
        /// </summary>
        public async Task<bool> IsLeafCategoryAsync(string categoryId)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM Category
                WHERE ParentCategoryID = :CategoryID";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var count = await connection.ExecuteScalarAsync<int>(sql, new { CategoryID = categoryId });
            return count == 0;
        }

        /// <summary>
        /// 获取分类的完整路径
        /// </summary>
        private async Task<string> GetCategoryPathAsync(string categoryId)
        {
            var path = new List<string>();
            var currentId = categoryId;

            while (!string.IsNullOrEmpty(currentId))
            {
                var sql = @"
                    SELECT CategoryName, ParentCategoryID
                    FROM Category
                    WHERE CategoryID = :CategoryID";

                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { CategoryID = currentId });
                
                if (result == null) break;

                path.Insert(0, result.CategoryName);
                currentId = result.ParentCategoryID ?? string.Empty;
            }

            return string.Join(" / ", path);
        }

        /// <summary>
        /// 获取图书分类关联统计
        /// </summary>
        public async Task<Dictionary<string, int>> GetBookCategoryStatsAsync()
        {
            var sql = @"
                SELECT 
                    c.CategoryName,
                    COUNT(bc.ISBN) as BookCount
                FROM Category c
                LEFT JOIN Book_Classify bc ON c.CategoryID = bc.CategoryID
                GROUP BY c.CategoryID, c.CategoryName
                ORDER BY BookCount DESC, c.CategoryName";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var results = await connection.QueryAsync<dynamic>(sql);
            
            return results.ToDictionary(r => (string)r.CategoryName, r => (int)r.BookCount);
        }
    }
}
