CREATE OR REPLACE VIEW announcement_view AS
SELECT
    a.AnnouncementID,
    a.Title,
    a.Content,
    a.CreateTime,
    a.TargetGroup,
    a.Status,
    a.LibrarianID,
    l.Name AS LibrarianName
FROM Announcement a
JOIN Librarian l ON a.LibrarianID = l.LibrarianID;