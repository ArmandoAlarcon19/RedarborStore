using RedarborStore.Domain.Entities;

namespace RedarborStore.Application.Tests.Fixtures;

public static class ProductFixture
{
    public static Product CreateProduct(
        int id = 1,
        string name = "Laptop",
        string? description = "Gaming laptop 16GB",
        decimal price = 1299.99m,
        int stock = 50,
        int categoryId = 1,
        bool isActive = true)
    {
        return new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
            CategoryId = categoryId,
            CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
    }

    public static List<Product> CreateProductList()
    {
        return new List<Product>
        {
            CreateProduct(1, "Laptop", "Gaming laptop", 1299.99m, 50, 1),
            CreateProduct(2, "T-Shirt", "Cotton t-shirt", 19.99m, 200, 2),
            CreateProduct(3, "Coffee Beans", "Premium arabica", 24.50m, 100, 3),
        };
    }

    public static List<Product> CreateProductListByCategory(int categoryId)
    {
        return CreateProductList()
            .Where(p => p.CategoryId == categoryId)
            .ToList();
    }

    public static List<Product> CreateEmptyList() => new List<Product>();

    public static (IEnumerable<Product> Items, int TotalCount) CreatePaginatedList(
        int pageNumber = 1,
        int pageSize = 10)
    {
        var all = CreateProductList();
        var items = all
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return (items, all.Count);
    }

    public static List<Product> CreateLargeProductList(int count = 25)
    {
        return Enumerable.Range(1, count)
            .Select(i => CreateProduct(i, $"Product {i}", $"Description {i}", 10m * i, i * 5, (i % 3) + 1))
            .ToList();
    }

    public static (IEnumerable<Product> Items, int TotalCount) CreateLargePaginatedList(
        int pageNumber = 1,
        int pageSize = 10,
        int totalItems = 25)
    {
        var all = CreateLargeProductList(totalItems);
        var items = all
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return (items, all.Count);
    }
}