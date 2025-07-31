CREATE OR REPLACE PROCEDURE ModifyBooklist_Name(
    p_booklist_id IN INT,
    p_booklist_name IN VARCHAR2
) AS
BEGIN
    UPDATE Booklist
    SET BooklistName = p_booklist_name
    WHERE BooklistID = p_booklist_id;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('书单名称修改成功');
END ModifyBooklist_Name;
/

CREATE OR REPLACE PROCEDURE ModifyBooklist_Intro(
    p_booklist_id IN INT,
    p_booklist_intro IN CLOB
) AS
BEGIN
    UPDATE Booklist
    SET BooklistIntroduction = p_booklist_intro
    WHERE BooklistID = p_booklist_id;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('书单简介修改成功');
END ModifyBooklist_Intro;
/
