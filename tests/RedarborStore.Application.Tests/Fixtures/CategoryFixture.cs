using RedarborStore.Domain.Entities;

namespace RedarborStore.Application.Tests.Fixtures;

public static class CategoryFixture
{
    public static Category CreateCategory(
        int id = 1,
        string name = "Electronics",
        string? description = "Electronic devices",
        bool isDeleted = true)
    {
        return new Category
        {
            Id = id,
            Name = name,
            Description = description,
            CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
    }

    public static List<Category> CreateCategoryList()
    {
        return new List<Category>
        {
            CreateCategory(1, "Electronics", "Electronic devices"),
            CreateCategory(2, "Clothing", "Apparel and fashion"),
            CreateCategory(3, "Food & Beverages", "Consumable products"),
        };
    }

    public static List<Category> CreateEmptyList() => new List<Category>();

    /// <summary>
    /// Simula la respuesta paginada del repository: retorna una página
    /// de la lista completa y el totalCount.
    /// </summary>
    public static (IEnumerable<Category> Items, int TotalCount) CreatePaginatedList(
        int pageNumber = 1,
        int pageSize = 10)
    {
        var all = CreateCategoryList();
        var items = all
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return (items, all.Count);
    }
    public static List<Category> CreateLargeCategoryList(int count = 25)
    {
        return Enumerable.Range(1, count)
            .Select(i => CreateCategory(i, $"Category {i}", $"Description {i}"))
            .ToList();
    }

    public static (IEnumerable<Category> Items, int TotalCount) CreateLargePaginatedList(
        int pageNumber = 1,
        int pageSize = 10,
        int totalItems = 25)
    {
        var all = CreateLargeCategoryList(totalItems);
        var items = all
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return (items, all.Count);
    }
}