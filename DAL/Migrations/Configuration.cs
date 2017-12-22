namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    using DAL.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.BookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.BookContext context)
        {
            context.Genres.AddOrUpdate(x => x.ID,
                new Genre { ID = 1, Name = "Khoa học" },
                new Genre { ID = 2, Name = "Kinh dị" },
                new Genre { ID = 3, Name = "Kỹ năng mềm" },
                new Genre { ID = 4, Name = "Truyện tranh" }
            );
            context.Publishers.AddOrUpdate(x => x.ID,
                new Publisher { ID = 1, Name = "Kim Đồng", Address = "abc", Phone = "123", AccountNumber = "02312" },
                new Publisher { ID = 2, Name = "Trẻ", Address = "xyz", Phone = "456", AccountNumber = "1321" },
                new Publisher { ID = 3, Name = "Tổng hợp", Address = "xyz", Phone = "456", AccountNumber = "1321" },
                new Publisher { ID = 4, Name = "Đại học Quốc gia", Address = "xyz", Phone = "456", AccountNumber = "1321" }
            );
            context.Agencies.AddOrUpdate(x => x.ID,
                new Agency { ID = 1, Name = "Đại lý 1", Address = "uerowi", Phone = "2343", AccountNumber = "01232312" },
                new Agency { ID = 2, Name = "Đại lý 2", Address = "puqp", Phone = "4532526", AccountNumber = "1432321" },
                new Agency { ID = 3, Name = "Đại lý 3", Address = "fads", Phone = "4532526", AccountNumber = "1432321" },
                new Agency { ID = 4, Name = "Đại lý 4", Address = "puqwerwep", Phone = "4532526", AccountNumber = "1432321" }
            );
            context.Books.AddOrUpdate(x => x.ID,
                new Book
                {
                    ID = 1,
                    PublisherID = 1,
                    GenreID = 3,
                    Name = "Đắc nhân tâm",
                    Author = "Dale Carnegie",
                    PublicationDate = DateTime.Parse("2008-09-01"),
                    SellingPrice = 100000,
                    PurchasePrice = 80000
                },
                 new Book
                 {
                     ID = 2,
                     PublisherID = 1,
                     GenreID = 3,
                     Name = "Quẳng gánh lo đi mà vui sống",
                     Author = "Dale Carnegie",
                     PublicationDate = DateTime.Parse("2010-7-8"),
                     SellingPrice = 89000,
                     PurchasePrice = 70000
                 },
                 new Book
                 {
                     ID = 3,
                     PublisherID = 2,
                     GenreID = 4,
                     Name = "Doraemon",
                     Author = "Fujiko Fujio",
                     PublicationDate = DateTime.Parse("2001-11-24"),
                     SellingPrice = 23000,
                     PurchasePrice = 15000
                 },
                 new Book
                 {
                     ID = 4,
                     PublisherID = 2,
                     GenreID = 4,
                     Name = "Thám tử lừng danh Conan",
                     Author = "Aoyama Gosho",
                     PublicationDate = DateTime.Parse("2002-2-10"),
                     SellingPrice = 28000,
                     PurchasePrice = 18000
                 },
                 new Book
                 {
                     ID = 5,
                     PublisherID = 3,
                     GenreID = 1,
                     Name = "Lược sử thời gian",
                     Author = "Stephen Hawking",
                     PublicationDate = DateTime.Parse("2008-2-13"),
                     SellingPrice = 70000,
                     PurchasePrice = 65000
                 },
                 new Book
                 {
                     ID = 6,
                     PublisherID = 3,
                     GenreID = 1,
                     Name = "Nguồn gốc các loài",
                     Author = "Charles Darwin",
                     PublicationDate = DateTime.Parse("2011-7-11"),
                     SellingPrice = 83000,
                     PurchasePrice = 72500
                 },
                 new Book
                 {
                     ID = 7,
                     PublisherID = 4,
                     GenreID = 2,
                     Name = "IT",
                     Author = "Stephen King",
                     PublicationDate = DateTime.Parse("2012-12-31"),
                     SellingPrice = 105000,
                     PurchasePrice = 90000
                 },
                 new Book
                 {
                     ID = 8,
                     PublisherID = 4,
                     GenreID = 2,
                     Name = "Dracula",
                     Author = "Bram Stocker",
                     PublicationDate = DateTime.Parse("2014-2-13"),
                     SellingPrice = 125000,
                     PurchasePrice = 100000
                 }
            );
            context.Receipts.AddOrUpdate(x => x.ID,
                 new Receipt{
                     ID = 1,
                     PublisherID = 1, Total = 15000000, Date = DateTime.Parse("2017-10-28 10:53"), Description = ""
                 , Status = true},
                 new Receipt{
                     ID = 2,
                     PublisherID = 2, Total = 3300000, Date = DateTime.Parse("2017-10-29 09:24"), Description = ""
                 , Status = true},
                 new Receipt{
                     ID = 3,
                     PublisherID = 3, Total = 13750000, Date = DateTime.Parse("2017-10-29 11:36"), Description = ""
                 , Status = true},
                 new Receipt{
                     ID = 4,
                     PublisherID = 4, Total = 19000000, Date = DateTime.Parse("2017-10-30 08:12"), Description = ""
                 , Status = true}
            );
            context.ReceiptDetails.AddOrUpdate(x => new { x.ReceiptID, x.BookID },
                 new ReceiptDetail { ReceiptID = 1, BookID = 1, Quantity = 100, UnitPrice = 80000 },
                 new ReceiptDetail { ReceiptID = 1, BookID = 2, Quantity = 100, UnitPrice = 70000 },
                 new ReceiptDetail { ReceiptID = 2, BookID = 3, Quantity = 100, UnitPrice = 15000 },
                 new ReceiptDetail { ReceiptID = 2, BookID = 4, Quantity = 100, UnitPrice = 18000 },
                 new ReceiptDetail { ReceiptID = 3, BookID = 5, Quantity = 100, UnitPrice = 65000 },
                 new ReceiptDetail { ReceiptID = 3, BookID = 6, Quantity = 100, UnitPrice = 72500 },
                 new ReceiptDetail { ReceiptID = 4, BookID = 7, Quantity = 100, UnitPrice = 90000 },
                 new ReceiptDetail { ReceiptID = 4, BookID = 8, Quantity = 100, UnitPrice = 100000 }
            );
            context.Invoices.AddOrUpdate(x => x.ID,
                 new Invoice{
                     ID = 1,
                     AgencyID = 1, Total = 2335000, Date = DateTime.Parse("2017-11-1 07:55"), Description = ""
                 , Status = true},
                 new Invoice{
                     ID = 2,
                     AgencyID = 2, Total = 3865000, Date = DateTime.Parse("2017-11-2 14:33"), Description = ""
                 , Status = true}
            );
            context.InvoiceDetails.AddOrUpdate(x => new { x.InvoiceID, x.BookID },
                 new InvoiceDetail { InvoiceID = 1, BookID = 1, Quantity = 10, UnitPrice = 100000 },
                 new InvoiceDetail { InvoiceID = 1, BookID = 2, Quantity = 15, UnitPrice = 89000 },
                 new InvoiceDetail { InvoiceID = 2, BookID = 3, Quantity = 25, UnitPrice = 23000 },
                 new InvoiceDetail { InvoiceID = 2, BookID = 4, Quantity = 30, UnitPrice = 28000 },
                 new InvoiceDetail { InvoiceID = 2, BookID = 5, Quantity = 35, UnitPrice = 70000 }
            );
            context.Stocks.AddOrUpdate(x => x.ID,
                 new Stock { ID = 1, BookID = 1, Quantity = 100, Date = DateTime.Parse("2017-10-28 10:53") },
                 new Stock { ID = 2, BookID = 2, Quantity = 100, Date = DateTime.Parse("2017-10-28 10:53") },
                 new Stock { ID = 3, BookID = 3, Quantity = 100, Date = DateTime.Parse("2017-10-29 09:24") },
                 new Stock { ID = 4, BookID = 4, Quantity = 100, Date = DateTime.Parse("2017-10-29 09:24") },
                 new Stock { ID = 5, BookID = 5, Quantity = 100, Date = DateTime.Parse("2017-10-29 11:36") },
                 new Stock { ID = 6, BookID = 6, Quantity = 100, Date = DateTime.Parse("2017-10-29 11:36") },
                 new Stock { ID = 7, BookID = 7, Quantity = 100, Date = DateTime.Parse("2017-10-30 08:12") },
                 new Stock { ID = 8, BookID = 8, Quantity = 100, Date = DateTime.Parse("2017-10-30 08:12") },
                 // xuat
                 new Stock { ID = 9, BookID = 1, Quantity = 90, Date = DateTime.Parse("2017-11-1 07:55") },
                 new Stock { ID = 10, BookID = 2, Quantity = 85, Date = DateTime.Parse("2017-11-1 07:55") },
                 new Stock { ID = 11, BookID = 3, Quantity = 75, Date = DateTime.Parse("2017-11-2 14:33") },
                 new Stock { ID = 12, BookID = 4, Quantity = 70, Date = DateTime.Parse("2017-11-2 14:33") },
                 new Stock { ID = 13, BookID = 5, Quantity = 65, Date = DateTime.Parse("2017-11-2 14:33") }
            );
            context.AgencyBookDebts.AddOrUpdate(x => x.ID,
                 new AgencyBookDebt { ID = 1, AgencyID = 1, BookID = 1, Quantity = 10, Date = DateTime.Parse("2017-11-1 07:55") },
                 new AgencyBookDebt { ID = 2, AgencyID = 1, BookID = 2, Quantity = 15, Date = DateTime.Parse("2017-11-1 07:55") },
                 new AgencyBookDebt { ID = 3, AgencyID = 2, BookID = 3, Quantity = 25, Date = DateTime.Parse("2017-11-2 14:33") },
                 new AgencyBookDebt { ID = 4, AgencyID = 2, BookID = 4, Quantity = 30, Date = DateTime.Parse("2017-11-2 14:33") },
                 new AgencyBookDebt { ID = 5, AgencyID = 2, BookID = 5, Quantity = 35, Date = DateTime.Parse("2017-11-2 14:33") }
            );
            context.AgencyDebts.AddOrUpdate(x => x.ID,
                 new AgencyDebt { ID = 1, AgencyID = 1, Amount = 2335000, Date = DateTime.Parse("2017-11-1 07:55") },
                 new AgencyDebt { ID = 2, AgencyID = 2, Amount = 3865000, Date = DateTime.Parse("2017-11-2 14:33") }
            );
            context.BookDebts.AddOrUpdate(x => x.ID,
                 new BookDebt { ID = 1, PublisherID = 1, BookID = 1, Quantity = 100, Date = DateTime.Parse("2017-10-28 10:53") },
                 new BookDebt { ID = 2, PublisherID = 1, BookID = 2, Quantity = 100, Date = DateTime.Parse("2017-10-28 10:53") },
                 new BookDebt { ID = 3, PublisherID = 2, BookID = 3, Quantity = 100, Date = DateTime.Parse("2017-10-29 09:24") },
                 new BookDebt { ID = 4, PublisherID = 2, BookID = 4, Quantity = 100, Date = DateTime.Parse("2017-10-29 09:24") },
                 new BookDebt { ID = 5, PublisherID = 3, BookID = 5, Quantity = 100, Date = DateTime.Parse("2017-10-29 11:36") },
                 new BookDebt { ID = 6, PublisherID = 3, BookID = 6, Quantity = 100, Date = DateTime.Parse("2017-10-29 11:36") },
                 new BookDebt { ID = 7, PublisherID = 4, BookID = 7, Quantity = 100, Date = DateTime.Parse("2017-10-30 08:12") },
                 new BookDebt { ID = 8, PublisherID = 4, BookID = 8, Quantity = 100, Date = DateTime.Parse("2017-10-30 08:12") }
            );
            context.Debts.AddOrUpdate(x => x.ID,
                 new Debt { ID = 1, PublisherID = 1, Amount = 15000000, Date = DateTime.Parse("2017-10-28 10:53") },
                 new Debt { ID = 2, PublisherID = 2, Amount = 3300000, Date = DateTime.Parse("2017-10-29 09:24") },
                 new Debt { ID = 3, PublisherID = 3, Amount = 13750000, Date = DateTime.Parse("2017-10-29 11:36") },
                 new Debt { ID = 4, PublisherID = 4, Amount = 19000000, Date = DateTime.Parse("2017-10-30 08:12") }
            );
        }
    }
}
