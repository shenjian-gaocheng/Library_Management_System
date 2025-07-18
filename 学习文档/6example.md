# 示例项目： book组 图书的查找功能，先分支
## 1.目录

    library_system/
    ├── frontend/
    │   └── modules/
    │       └── book/   （🔵 图书小组前端）
    │           ├── pages/            ← BookSearchPage.vue（图书搜索页面）
    │           ├── components/       ← BookCard.vue / BookSearchBar.vue（展示与输入）
    │           ├── api.js            ← 封装接口请求：getBooks(query)
    │           └── index.js
    │
    ├── backend/
    │   ├── Controllers/
    │   │   └── Book/
    │   │       └── BookController.cs （🔵 图书小组后端控制器，接收前端请求）
    │   ├── Services/
    │   │   └── Book/
    |   │       └── BookService.cs        （🔵 查询逻辑封装：模糊搜索、分页、分类）
    │   ├── DTOs/
    │   │   └── Book/
    │   │       └── BookDetailDto.cs  （🔵 查询结果结构定义）
    │   ├── Repositories/
    │   │   └── Book/
    |   │       └── BookRepository.cs     （🔵 编写 SQL 查询数据库）
    │   ├── Models/
    │   |   └── Book.cs               （🔵 映射数据库）
    |   ├── Program.cs
    |   ├── Startup.cs
    |   └── appsettings.json
    |
    ├── database/
    │   ├── views/
    │   │   └── book/
    │   │       └── book_detail_view.sql ← 🟡 公共视图（联合Book + BookInfo + 分类 + 位置）
    │   └── init.sql （包含上述视图）

## 2.sql层
### 1.book_detail_view.sql
    CREATE OR REPLACE VIEW book_detail_view AS
    SELECT
        b.BookID,
        b.Status,
        b.ShelfID,
        b.BuildingID,
        b.ISBN,
        bi.Title,
        bi.Author,
        bi.Stock
    FROM Book b
    JOIN BookInfo bi ON b.ISBN = bi.ISBN;
### 2.在init.sql中包含上述视图
    @views/book/book_detail_view.sql
### 3.运行方法
- 开发时：book_detail_view.sql拖拽到sql developer中，点击运行脚本

    退出sql developer时，若选择提交更改，本次在sql developer中执行的文件都会被服务器永久保存

    若选择回退更改，本次在sql developer中执行的文件不会被保存，Oracle回退到你本次所有操作之前
- 移植到其他项目时：在服务器一键执行执行init.sql脚本，注册所有sql语句
### 4.我已经插入部分数据，数据先不要自行insert，过几天统一导入

## 3.后端层
### 1.BookController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _service;

        public BookController(BookService service)
        {
            _service = service;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<BookDetailDto>> Search(string keyword)
        {
            return await _service.SearchBooksAsync(keyword ?? "");
        }
    }
### 2.BookService.cs
    public class BookService
    {
        private readonly BookRepository _repository;

        public BookService(BookRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<BookDetailDto>> SearchBooksAsync(string keyword)
        {
            return _repository.SearchBooksAsync(keyword);
        }
    }
### 3.BookRepository.cs
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
### 4.DTO BookDetailDto.cs
    public class BookDetailDto
    {
        public string BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
    }
### 5.Program.cs
    var builder = WebApplication.CreateBuilder(args);

    // 添加控制器
    builder.Services.AddControllers();

    // 连接字符串
    var connectionString = builder.Configuration.GetConnectionString("OracleDB");

    // 注册服务
    builder.Services.AddSingleton(new BookRepository(connectionString));
    builder.Services.AddTransient<BookService>();

    var app = builder.Build();
    app.UseRouting();
    app.MapControllers();

    app.Run();
### 6.appsettings.json
    {
        "ConnectionStrings": {
            "OracleDB": "User Id=your_user;Password=your_pass;Data Source=your_oracle_db"
        }
    }
### 7.运行方法
- 开发时：运行在本地
```
export ASPNETCORE_ENVIRONMENT=Development
dotnet watch run
```
- 答辩前运行在服务器
```
export ASPNETCORE_ENVIRONMENT=Production
dotnet watch run
```
## 4.前端层
### 1.api.js
    export function getBooks(keyword) {
        return http.get('/api/book/search', { params: { keyword } });
    }
### 2.BookSearchPage.vue
    <script setup>
    import { ref } from 'vue'
    import { getBooks } from './api.js'

    const keyword = ref('')
    const books = ref([])

    const search = async () => {
    const res = await getBooks(keyword.value)
    books.value = res.data
    }
    </script>

    <template>
    <input v-model="keyword" placeholder="请输入书名或作者" />
    <button @click="search">搜索</button>

    <div v-for="book in books" :key="book.BookID">
        <p>{{ book.Title }} - {{ book.Author }}</p>
    </div>
    </template>
### 3.运行方法
- 开发时
```
npm run dev
```
- 答辩前：运行在服务器上
```
npm run build
```
