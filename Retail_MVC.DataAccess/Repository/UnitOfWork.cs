using Retail_MVC.DataAccess.Data;
using Retail_MVC.DataAccess.Repository.IRepository;
using Retail_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail_MVC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IVendorRepository Vendor { get; private set; }



        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product =new ProductRepository(_db);
            Vendor = new VendorRepository(_db);
        }
       

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
