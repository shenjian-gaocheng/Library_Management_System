CREATE OR REPLACE PROCEDURE DeleteBooklist(
    p_booklist_id IN INT
) AS
BEGIN
    DELETE FROM Booklist WHERE BooklistID = p_booklist_id;
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('书单删除成功');
END DeleteBooklist;
/
