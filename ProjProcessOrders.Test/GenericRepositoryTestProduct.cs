using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Infrastructure.Context;
using ProjProcessOrders.Infrastructure.Repositories;

namespace ProjProcessOrders.Test;

public class GenericRepositoryTestProduct : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly GenericRepository<Product, int> _productRepository;


    public GenericRepositoryTestProduct()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ApplicationDbContext(options);

        var mockLogger = new Mock<ILogger<GenericRepository<Product, int>>>();
        _productRepository = new GenericRepository<Product, int>(_dbContext, mockLogger.Object);
    }

    [Theory]
    [InlineData("Teste-produto", 30, 34.69)]
    public async Task InsertAsync_ShouldAddEntityAndSaveChanges(string name, int productQuantity, decimal productPrice)
    {
        // Arrange
        var entity = new Product { ProductName = name, ProductQuantity = productQuantity, ProductPrice = productPrice };

        // Act
        var result = await _productRepository.InsertAsync(entity);

        // Assert
        var insertedEntity = await _dbContext.Set<Product>().FindAsync(result.Id);
        Assert.NotNull(insertedEntity);
        Assert.Equal(entity.ProductName, insertedEntity.ProductName);
        Assert.Equal(entity.ProductQuantity, insertedEntity.ProductQuantity);
        Assert.Equal(entity.ProductPrice, insertedEntity.ProductPrice);
        
    }
 
    
    [Theory]
    [InlineData("Teste-produto", 30, 34.69)]
    public async Task UpdateAsync_ShouldUpdateEntityAndSaveChanges(string name, int productQuantity, decimal productPrice)
    {
        // Arrange
        var entity = new Product { ProductName = name, ProductQuantity = productQuantity, ProductPrice = productPrice };
        _dbContext.Set<Product>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        entity.ProductName = "Novo nome";
        entity.ProductQuantity = 50;
        entity.ProductPrice = 49.99m;
        var result = await _productRepository.UpdateAsync(entity);

        // Assert
        var updatedEntity = await _dbContext.Set<Product>().FindAsync(result.Id);
        Assert.NotNull(updatedEntity);
        Assert.Equal(entity.ProductName, updatedEntity.ProductName);
        Assert.Equal(entity.ProductQuantity, updatedEntity.ProductQuantity);
        Assert.Equal(entity.ProductPrice, updatedEntity.ProductPrice);
    }
    
    [Theory]
    [InlineData("Teste-produto", 30, 34.69)]
    public async Task DeleteAsync_ShouldDeleteEntityAndSaveChanges(string name, int productQuantity, decimal productPrice)
    {
        // Arrange
        var entity = new Product { ProductName = name, ProductQuantity = productQuantity, ProductPrice = productPrice };
        _dbContext.Set<Product>().Add(entity);
        
        // Act
        await _productRepository.DeleteAsync(entity);
        
        // Assert
        var deletedEntity = await _dbContext.Set<Product>().FindAsync(entity.Id);
        Assert.Null(deletedEntity);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnEntity()
    {
        // Arrange
        var entity1 = new Product { ProductName = "Test1", ProductQuantity = 30, ProductPrice = 34.69m };
        var entity2 = new Product { ProductName = "Test2", ProductQuantity = 30, ProductPrice = 34.69m };
        _dbContext.Set<Product>().Add(entity1);
        _dbContext.Set<Product>().Add(entity2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.Queryable(x => x.Id > 0).ToListAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Id == entity1.Id);
        Assert.Contains(result, x => x.ProductName == entity1.ProductName);
        Assert.Contains(result, x => x.ProductQuantity == entity1.ProductQuantity);
        
        Assert.Contains(result, x => x.Id == entity2.Id);
        Assert.Contains(result, x => x.ProductName == entity2.ProductName);
        Assert.Contains(result, x => x.ProductQuantity == entity2.ProductQuantity);
    }
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}