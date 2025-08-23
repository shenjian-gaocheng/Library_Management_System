-- 文件: database/views/admin/librarian_view.sql
CREATE OR REPLACE VIEW librarian_view AS
SELECT
    LibrarianID,
    Name,
    Permission
FROM Librarian;
-- 注意：为了安全，视图中绝对不能包含 Password 字段。