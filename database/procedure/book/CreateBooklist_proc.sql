CREATE OR REPLACE PROCEDURE CreateBooklist(
    p_list_code IN VARCHAR2,
    p_booklist_name IN VARCHAR2,
    p_booklist_intro IN CLOB,
    p_creator_id IN INT
) AS
BEGIN
    INSERT INTO Booklist (ListCode, BooklistName, BooklistIntroduction, CreatorID)
    VALUES (p_list_code, p_booklist_name, p_booklist_intro, p_creator_id);
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('书单添加成功');
END CreateBooklist;
/
