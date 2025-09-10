-- 测试搜索功能的SQL脚本
-- 请依次执行以下查询来诊断问题

-- 1. 检查BookInfo表是否有数据
SELECT 'BookInfo表记录数:' as Info, COUNT(*) as Count FROM BookInfo;

-- 2. 查看BookInfo表中的所有数据
SELECT ISBN, Title, Author FROM BookInfo ORDER BY ISBN;

-- 3. 检查Category表是否有数据
SELECT 'Category表记录数:' as Info, COUNT(*) as Count FROM Category;

-- 4. 查看Category表中的所有数据
SELECT CategoryID, CategoryName FROM Category ORDER BY CategoryID;

-- 5. 检查Book_Classify表是否有数据
SELECT 'Book_Classify表记录数:' as Info, COUNT(*) as Count FROM Book_Classify;

-- 6. 查看Book_Classify表中的所有数据
SELECT ISBN, CategoryID, RelationNote FROM Book_Classify ORDER BY ISBN;

-- 7. 测试搜索查询（搜索包含"Crime"的图书）
SELECT 
    bi.ISBN, 
    bi.Title, 
    bi.Author,
    LISTAGG(c.CategoryName, ', ') WITHIN GROUP (ORDER BY c.CategoryName) AS Categories
FROM BookInfo bi
LEFT JOIN Book_Classify bc ON bi.ISBN = bc.ISBN
LEFT JOIN Category c ON bc.CategoryID = c.CategoryID
WHERE LOWER(bi.Title) LIKE '%crime%' OR LOWER(bi.Author) LIKE '%crime%'
GROUP BY bi.ISBN, bi.Title, bi.Author
ORDER BY bi.Title;

-- 8. 测试搜索查询（搜索包含"nietzsche"的图书）
SELECT 
    bi.ISBN, 
    bi.Title, 
    bi.Author,
    LISTAGG(c.CategoryName, ', ') WITHIN GROUP (ORDER BY c.CategoryName) AS Categories
FROM BookInfo bi
LEFT JOIN Book_Classify bc ON bi.ISBN = bc.ISBN
LEFT JOIN Category c ON bc.CategoryID = c.CategoryID
WHERE LOWER(bi.Title) LIKE '%nietzsche%' OR LOWER(bi.Author) LIKE '%nietzsche%'
GROUP BY bi.ISBN, bi.Title, bi.Author
ORDER BY bi.Title;
