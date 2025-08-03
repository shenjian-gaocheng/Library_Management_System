-- 文件: database/views/admin/announcement_detail_view.sql
-- 功能: 创建一个公告详情视图，关联Librarian表以获取发布者姓名
CREATE OR REPLACE VIEW announcement_detail_view AS
SELECT
    a.AnnouncementID,
    a.Title,
    a.Content,
    a.CreateTime,
    a.TargetGroup,
    a.Status,
    a.LibrarianID,
    l.Name AS LibrarianName -- 关键：从Librarian表带出Name字段，并重命名为LibrarianName
FROM
    Announcement a
JOIN
    Librarian l ON a.LibrarianID = l.LibrarianID;