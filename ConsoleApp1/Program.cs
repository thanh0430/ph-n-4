using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

internal class Program
{
    public class Product
    {
        public int ID { set; get; }
        public string Name { set; get; }         // tên
        public double Price { set; get; }        // giá
        public string[] Colors { set; get; }     // các màu sắc
        public int Brand { set; get; }           // ID Nhãn hiệu, hãng
        public Product(int id, string name, double price, string[] colors, int brand)
        {
            ID = id; Name = name; Price = price; Colors = colors; Brand = brand;
        }
        // Lấy chuỗi thông tin sản phẳm gồm ID, Name, Price
        override public string ToString()
           => $"{ID,3} {Name,12} {Price,5} {Brand,2} {string.Join(",", Colors)}";

    }
    public class Brand
    {
        public string Name { set; get; }
        public int ID { set; get; }
    }

    private static void Main(string[] args)
    {
        var brands = new List<Brand>() {
            new Brand{ID = 1, Name = "Công ty AAA"},
            new Brand{ID = 2, Name = "Công ty BBB"},
            new Brand{ID = 4, Name = "Công ty CCC"},
        };

       var products = new List<Product>()
        {
            new Product(1, "Bàn trà",    400, new string[] {"Xám", "Xanh"},         2),
            new Product(2, "Tranh treo", 400, new string[] {"Vàng", "Xanh"},        1),
            new Product(3, "Đèn trùm",   500, new string[] {"Trắng"},               3),
            new Product(4, "Bàn học",    200, new string[] {"Trắng", "Xanh"},       1),
            new Product(5, "Túi da",     300, new string[] {"Đỏ", "Đen", "Vàng"},   2),
            new Product(6, "Giường ngủ", 500, new string[] {"Trắng"},               2),
            new Product(7, "Tủ áo",      600, new string[] {"Trắng"},               3),
        };
        //select 
        var kq = from p in products 
                 select p;
       kq.ToList().ForEach(p=>Console.WriteLine(p));

        //min,max,sum, avg
        var kq2 = from p in products
                  select new
                  {
                      ten=p.Name,
                      price = products.Min(p=>p.Price), // mã sum avg tương tự
                  };
        kq2.ToList().ForEach(p => Console.WriteLine(p));

        //slip bỏ đi nhwunxg phần tử đầu tiên lấy nhwunxg pt còn lại
        var kq3 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.Skip(2),
                  };
        kq3.ToList().ForEach(p => Console.WriteLine(p));

        //TAKE lấy nhwunxg sản phẩm đầu tiên
        var kq4 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.Take(4), 
                  };
        kq4.ToList().ForEach(p => Console.WriteLine(p));

        //single of singledefault: lấy nhwung phần tử có 1 giá trị single sẽ trả 1 Exception khi có nhiều hơn 2 phần tử haowcj k tìm thấy
        // còn singledefault sẽ trả ra là null khi k tìm thấy pt 
        var kq5 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.SingleOrDefault(p=>p.Price==200), 
                  };
        kq5.ToList().ForEach(p => Console.WriteLine(p));

     //First, FirstOrDeault: First trả về phần tử đầu tiên từ tập hợp thỏa mãn điều kiện được chỉ định
        //Nếu không có phần tử nào trong tập hợp thỏa mãn điều kiện, phương thức First() sẽ ném một ngoại lệ
        //FirstOrDeault: trả về phần tử đầu tiên từ tập hợp thỏa mãn điều kiện được chỉ định.
        //Tuy nhiên, nếu không có phần tử nào trong tập hợp thỏa mãn điều kiện, phương thức FirstOrDefault() sẽ trả về giá trị mặc định(default)
        //của kiểu dữ liệu của phần tử đó(ví dụ: null cho kiểu tham chiếu, 0 cho kiểu số nguyên, và false cho kiểu boolean
                var kq9 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.FirstOrDefault().Price,
                  };
        kq9.ToList().ForEach(p => Console.WriteLine(p));

        //Last & LastOrDefault tương trự như First, FirstOrDeault nhưng Last & LastOrDefault sẽ trả về phần tử cuối cùng từ 1 tập howpj thỏa mãn đk
        var kq10 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.FirstOrDefault().Price,
                  };
        kq10.ToList().ForEach(p => Console.WriteLine(p));

        //Any trả về true nếu thỏa mãn đk nào đó
        var kq6 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.Any(p=>p.Price==600),
                  };
        kq6.ToList().ForEach(p => Console.WriteLine(p));

        //all trả về true nếu tất cả trả về đúng theo đk
        var kq7 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.All(p => p.Price >=200),
                  };
        kq7.ToList().ForEach(p => Console.WriteLine(p));

        // count đếm tất cả các phần tử
        var kq8 = from p in products
                  select new
                  {
                      ten = p.Name,
                      price = products.Count(p => p.Price >= 200),
                  };
        kq8.ToList().ForEach(p => Console.WriteLine(p));
        // co đk
        var kq1 = from p in products
                  where p.Price >= 200
                  group p by p.Price into gr
                  orderby gr.Key
                  select gr;
        kq1.ToList().ForEach(p => Console.WriteLine(p));

        var qr= from p  in products join b in brands on p.Brand equals b.ID
                select new
                {
                    name = p.Name,
                    brands= b.Name
                };
        qr.ToList().ForEach(p => Console.WriteLine(p));
    }
}