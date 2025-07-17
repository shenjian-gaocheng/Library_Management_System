
-- Step 1: Building
INSERT INTO Building (BuildingID, BuildingName, Address, TotalFloors, OpenHours)
VALUES ('BLDG1', '主图书馆A', '校内东区', 3, '08:00-21:00');

INSERT INTO Building (BuildingID, BuildingName, Address, TotalFloors, OpenHours)
VALUES ('BLDG2', '主图书馆B', '校内西区', 4, '08:00-20:00');

-- Step 2: Bookshelf
INSERT INTO Bookshelf (BuildingID, ShelfCode, Floor, Zone)
VALUES ('BLDG1', 'S01', 1, 'A区');

INSERT INTO Bookshelf (BuildingID, ShelfCode, Floor, Zone)
VALUES ('BLDG1', 'S02', 1, 'B区');

INSERT INTO Bookshelf (BuildingID, ShelfCode, Floor, Zone)
VALUES ('BLDG2', 'S03', 1, 'C区');

-- Step 3: BookInfo

INSERT INTO BookInfo (ISBN, Title, Author, Stock)
VALUES ('9780001', '数据库系统概论', '王珊', 5);

INSERT INTO BookInfo (ISBN, Title, Author, Stock)
VALUES ('9780002', '操作系统原理', '汤小丹', 4);

INSERT INTO BookInfo (ISBN, Title, Author, Stock)
VALUES ('9780003', '计算机网络', '谢希仁', 6);

-- Step 4: Book
INSERT INTO Book (BookID, Status, ShelfID, BuildingID, ISBN)
VALUES ('B001', '正常', 'S01', 'BLDG1', '9780001');

INSERT INTO Book (BookID, Status, ShelfID, BuildingID, ISBN)
VALUES ('B002', '借出', 'S02', 'BLDG1', '9780002');

INSERT INTO Book (BookID, Status, ShelfID, BuildingID, ISBN)
VALUES ('B003', '正常', 'S03', 'BLDG2', '9780003');
