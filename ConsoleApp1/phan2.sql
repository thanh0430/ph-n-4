--tạo bảng
CREATE TABLE Category (
  CategoryId INT IDENTITY(1,1) PRIMARY KEY,
  CategoryName VARCHAR(250) NOT NULL
);

CREATE TABLE SanPham (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  ProductName VARCHAR(250) NOT NULL,
  Price DECIMAL(10,2) NOT NULL,  
  Image VARCHAR(250) DEFAULT NULL,  
  Description VARCHAR(250) NOT NULL, 
  Quantity INT NOT NULL,
  CategoryId INT,  -- Thêm cột CategoryId làm khóa ngoại
  CONSTRAINT FK_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId),
  CONSTRAINT Check_Price CHECK (Price >= 0.00)  -- Đảm bảo rằng giá trị Price không âm
);


--them du lieu vao bang
-- Chèn dữ liệu vào bảng SanPham
-- Chèn dữ liệu vào bảng Category
INSERT INTO Category (CategoryName)
VALUES 
('Danh mục 1'),
('Danh mục 2');

-- Chèn dữ liệu vào bảng SanPham với CategoryId liên kết
INSERT INTO SanPham (ProductName, Price, Image, Description, Quantity, CategoryId)
VALUES 
('Sản phẩm 1', 111111, 'image1.jpg', 'Mô tả sản phẩm 1', 11, 1),  -- Liên kết với CategoryId 1
('Sản phẩm 2', 222222, 'image2.jpg', 'Mô tả sản phẩm 2', 22, 2);  -- Liên kết với CategoryId 2



ALTER TABLE SanPham
ADD FOREIGN KEY (CategryId) REFERENCES Category(CategryId);

--thêm cột mới vào bảng
ALTER TABLE SanPham 
ADD CategryId int;

--thay đổi kiểu dữ liệu của cột
ALTER TABLE SanPham ALTER COLUMN Price  float;

--xóa các cột 
ALTER TABLE SanPham
DROP Price, Price;

--update tên table
EXEC sp_rename 'SanPham', 'SanPham1';
--select 
select Id, Price from SanPham where Price>500
GROUP BY
Id, Price
ORDER BY
Id;

--inner join
SELECT
   *
FROM
 SanPham a inner join Category b on a.CategoryId = b.CategoryId 
 
 --union
 --SELECT Id,  ProductName, Price FROM SanPham a inner join Category b on a.CategryId = b.CategryId         
 --UNION        
 --SELECT Id, ProductName,Price FROM SanPham a LEFT JOIN Category b
         --ON a.CategryId = b.CategryId;

SELECT a.Id, a.ProductName, a.Price 
FROM SanPham a 
INNER JOIN Category b ON a.CategoryId = b.CategoryId
UNION
SELECT a.Id, a.ProductName, a.Price 
FROM SanPham a 
LEFT JOIN Category b ON a.CategoryId = b.CategoryId;

--avg 
SELECT AVG(Price) AS "Gia Trung Binh"
FROM SanPham;


CREATE TABLE LogPriceChanges (
  LogId INT IDENTITY(1,1) PRIMARY KEY,
  ProductId INT,  -- Sử dụng INT để liên kết với cột Id trong bảng SanPham
  OldPrice FLOAT NOT NULL,
  NewPrice FLOAT NOT NULL,
  ChangeDate DATE NOT NULL
);


--trigger
CREATE TRIGGER trg_LogPriceChanges
ON SanPham
AFTER UPDATE
AS
BEGIN
    -- Kiểm tra xem giá sản phẩm có thay đổi hay không
    IF UPDATE(Price)
    BEGIN
        -- Chèn bản ghi mới vào bảng LogPriceChanges
        INSERT INTO LogPriceChanges (ProductId, OldPrice, NewPrice, ChangeDate)
        SELECT d.Id, d.Price, i.Price, GETDATE()
        FROM inserted i
        INNER JOIN deleted d ON i.Id = d.Id
    END
END;


UPDATE SanPham 
SET
    Price = '2222222'
WHERE
    Id = 1;

-- funtion là một khối mã thực hiện một tác vụ cụ thể và có thể trả về một giá trị.
--Functions trong SQL Server giúp bạn đóng gói logic phức tạp và tái sử dụng mã trong các truy vấn SQL.
CREATE FUNCTION dbo.GetProducts (@ID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT a.Id, a.ProductName, a.Price
    FROM SanPham a
    WHERE Id = @ID
);
GO
SELECT * FROM dbo.GetProducts(1);

--store proceduce tập hợp một hoặc nhiều câu lệnh T-SQL thành một nhóm đơn vị xử lý logic và được lưu trữ trên Database Server
CREATE PROCEDURE uspProductList
AS
BEGIN
    SELECT
       a.Id, a.ProductName, a.Price
    FROM
        SanPham a
    ORDER BY
        ProductName;
END;

--xóa table 
DROP table SanPham; --nếu xóa bảng mà có khóa ngoại thì ta phải xóa khóa ngoại trước



