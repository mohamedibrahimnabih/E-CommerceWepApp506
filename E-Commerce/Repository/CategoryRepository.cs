using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        //ApplicationDbContext dbContext = new ApplicationDbContext();

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // CRUD operations
        public List<Category> GetAll(string? expression = null)
        {
            return expression == null ? dbContext.Categories.ToList() : dbContext.Categories.Include(expression).ToList();
        }

        public Category GetById(int categoryId)
        {
            return dbContext.Categories.Find(categoryId);
        }

        public void Create(Category category)
        {
            dbContext.Categories.Add(category);
        }

        public void Edit(Category category)
        {
            dbContext.Categories.Update(category);
        }

        public void Delete(Category category)
        {
            dbContext.Categories.Remove(category);
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
