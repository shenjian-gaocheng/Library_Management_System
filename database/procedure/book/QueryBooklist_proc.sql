CREATE OR REPLACE PROCEDURE QueryBooklist(
    p_creator_id IN INT,
    p_booklist_name IN VARCHAR2,
    p_booklist_id IN INT
) AS
BEGIN
    -- 查询并返回匹配的书单
    FOR r IN (
        SELECT BooklistID, BooklistName, BooklistIntroduction, CreatorID
        FROM Booklist
        WHERE 
            (p_creator_id IS NULL OR CreatorID = p_creator_id) AND
            (p_booklist_name IS NULL OR BooklistName LIKE '%' || p_booklist_name || '%') AND
            (p_booklist_id IS NULL OR BooklistID = p_booklist_id)
    ) LOOP
        -- 输出查询结果，可以根据实际需求进行修改
        DBMS_OUTPUT.PUT_LINE('书单 ID: ' || r.BooklistID || 
                             ', 书单名称: ' || r.BooklistName ||
                             ', 书单简介: ' || r.BooklistIntroduction);
    END LOOP;
END QueryBooklist;
/
