using RedarborStore.Domain.Entities;

namespace RedarborStore.Application.Tests.Fixtures;

public static class CategoryFixture
{
    public static Category CreateCategory(
        int id = 1,
        string name = "Electronics",
        string? description = "Electronic devices",
        bool isActive = true)
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
        return
        [
            CreateCategory(1, "Electronics", "Electronic devices"),
            CreateCategory(2, "Clothing", "Apparel and fashion"),
            CreateCategory(3, "Food & Beverages", "Consumable products"),
        ];
    }

    public static List<Category> CreateEmptyList() => [];
}