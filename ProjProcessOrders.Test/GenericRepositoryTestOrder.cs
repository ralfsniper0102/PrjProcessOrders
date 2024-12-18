using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Infrastructure.Context;
using ProjProcessOrders.Infrastructure.Repositories;

public class GenericRepositoryTestOrder : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly GenericRepository<Order, int> _orderRepository;


    public GenericRepositoryTestOrder()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ApplicationDbContext(options);

        var mockLogger = new Mock<ILogger<GenericRepository<Order, int>>>();
        _orderRepository = new GenericRepository<Order, int>(_dbContext, mockLogger.Object);
    }

    [Fact]
    public async Task InsertAsync_ShouldAddEntityAndSaveChanges()
    {
        // Arrange
        var entity = new Order { 
            ClientId = 1, 
            OrderProducts = new List<OrderProduct> { 
                new OrderProduct { 
                    ProductId = 1, 
                    Quantity = 1 
                },
                new OrderProduct { 
                    ProductId = 2, 
                    Quantity = 2 
                }
            }
        };

        // Act
        var result = await _orderRepository.InsertAsync(entity);

        // Assert
        var insertedEntity = await _dbContext.Set<Order>().FindAsync(result.Id);
        
        Assert.NotNull(insertedEntity);
        Assert.Equal(entity.ClientId, insertedEntity.ClientId);
        Assert.Equal(entity.OrderProducts.Count, insertedEntity.OrderProducts.Count);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntityAndSaveChanges()
    {
        // Arrange
        var entity = new Order { 
            ClientId = 1, 
            OrderProducts = new List<OrderProduct> { 
                new OrderProduct { 
                    ProductId = 1, 
                    Quantity = 1 
                },
                new OrderProduct { 
                    ProductId = 2, 
                    Quantity = 2 
                }
            }
        };
        _dbContext.Set<Order>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var getOrder = await _orderRepository.Queryable(x => x.Id == entity.Id).FirstOrDefaultAsync();
        getOrder.OrderProducts = new List<OrderProduct>
        {
            new OrderProduct
            {
                ProductId = 3,
                Quantity = 3
            },
            new OrderProduct
            {
                ProductId = 4,
                Quantity = 4
            }
        };

        var result = await _orderRepository.UpdateAsync(getOrder);

        // Assert
        var updatedEntity = await _dbContext.Set<Order>().FindAsync(result.Id);
        Assert.NotNull(updatedEntity);
        Assert.Equal(getOrder.ClientId, updatedEntity.ClientId);
        Assert.Equal(getOrder.OrderProducts.Count, updatedEntity.OrderProducts.Count);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntityAndSaveChanges()
    {
        // Arrange
        var entity = new Order { 
            ClientId = 1, 
            OrderProducts = new List<OrderProduct> { 
                new OrderProduct { 
                    ProductId = 1, 
                    Quantity = 1 
                },
                new OrderProduct { 
                    ProductId = 2, 
                    Quantity = 2 
                }
            }
        };
        _dbContext.Set<Order>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        await _orderRepository.DeleteAsync(entity);

        // Assert
        var deletedEntity = await _dbContext.Set<Order>().FindAsync(entity.Id);
        Assert.Null(deletedEntity);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEntity()
    {
        // Arrange
        var entity = new Order { 
            ClientId = 1, 
            OrderProducts = new List<OrderProduct> { 
                new OrderProduct { 
                    ProductId = 1, 
                    Quantity = 1 
                },
                new OrderProduct { 
                    ProductId = 2, 
                    Quantity = 2 
                }
            }
        };
        _dbContext.Set<Order>().Add(entity);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _orderRepository.Queryable(x => x.Id == entity.Id).FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.ClientId, result.ClientId);
        Assert.Equal(entity.OrderProducts.Count, result.OrderProducts.Count);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}