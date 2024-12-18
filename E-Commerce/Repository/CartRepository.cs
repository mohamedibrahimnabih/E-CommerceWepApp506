﻿using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository.IRepository;

namespace E_Commerce.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
