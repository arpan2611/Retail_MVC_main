﻿using Retail_MVC.DataAccess.Data;
using Retail_MVC.DataAccess.Repository.IRepository;
using Retail_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail_MVC.DataAccess.Repository
{
	public class CourierRepository : Repository<Courier>, ICourierRepository
    {
		private readonly ApplicationDbContext _db;
        public CourierRepository(ApplicationDbContext db):base(db) 
        {
            _db=db;
        }
       

		public void Update(Courier obj)
		{
			_db.couriers.Update(obj);
		}
	}
}
