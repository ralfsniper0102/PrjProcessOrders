using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Infrastructure.Context;
using ProjProcessOrders.Infrastructure.Repositories;

public class GenericRepositoryTestClient : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly GenericRepository<Client, int> _clientRepository;

    public GenericRepositoryTestClient()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ApplicationDbContext(options);

        var mockLogger = new Mock<ILogger<GenericRepository<Client, int>>>();
        _clientRepository = new GenericRepository<Client, int>(_dbContext, mockLogger.Object);
    }

    [Theory]
    [InlineData("Teste-cliente")]
    public async Task InsertAsync_ShouldAddEntityAndSaveChanges(string name)
    {
        // Arrange
        var entity = new Client { Name = name };

        // Act
        var result = await _clientRepository.InsertAsync(entity);

        // Assert
        var insertedEntity = await _dbContext.Set<Client>().FindAsync(result.Id);
        Assert.NotNull(insertedEntity);
        Assert.Equal(entity.Name, insertedEntity.Name);
    }
    
    [Theory]
    [InlineData("Teste-cliente")]
    public async Task UpdateAsync_ShouldUpdateEntityAndSaveChanges(string name)
    {
        // Arrange
        var entity = new Client { Name = name };
        _dbContext.Set<Client>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        entity.Name = "Updated";
        await _clientRepository.UpdateAsync(entity);

        // Assert
        var updatedEntity = await _dbContext.Set<Client>().FindAsync(1);
        Assert.NotNull(updatedEntity);
        Assert.Equal(entity.Name, updatedEntity.Name);
    }
    
    [Theory]
    [InlineData("Teste-cliente")]
    public async Task DeleteAsync_ShouldRemoveEntityAndSaveChanges(string name)
    {
        // Arrange
        var entity = new Client { Name = name };
        _dbContext.Set<Client>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        await _clientRepository.DeleteAsync(entity);

        // Assert
        var deletedEntity = await _dbContext.Set<Client>().FindAsync(1);
        Assert.Null(deletedEntity);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEntity()
    {
        // Arrange
        var entity = new Client { Name = "Test" };
        _dbContext.Set<Client>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _clientRepository.Queryable(x => x.Id == entity.Id).FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Name, result.Name);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEntityList()
    {
        // Arrange
        var entity1 = new Client { Name = "Test1" };
        var entity2 = new Client { Name = "Test2" };
        _dbContext.Set<Client>().Add(entity1);
        _dbContext.Set<Client>().Add(entity2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _clientRepository.Queryable(x => x.Id > 0).ToListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Id == entity1.Id);
        Assert.Contains(result, x => x.Name == entity1.Name);
        Assert.Contains(result, x => x.Id == entity2.Id);
        Assert.Contains(result, x => x.Name == entity2.Name);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
