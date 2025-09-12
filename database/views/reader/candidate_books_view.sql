CREATE OR REPLACE VIEW candidate_books_view AS
SELECT DISTINCT rc.ReaderID, bc.ISBN
FROM borrowed_ISBNs_view bi
         JOIN Book_Classify bc ON bi.ISBN = bc.ISBN
         JOIN (
    SELECT ReaderID, CategoryID
    FROM borrowed_ISBNs_view bi
             JOIN Book_Classify bc ON bi.ISBN = bc.ISBN
    GROUP BY ReaderID, CategoryID
) rc ON rc.CategoryID = bc.CategoryID
WHERE NOT EXISTS (
    SELECT 1 FROM borrowed_ISBNs_view bi2
    WHERE bi2.ReaderID = rc.ReaderID
      AND bi2.ISBN = bc.ISBN
)
