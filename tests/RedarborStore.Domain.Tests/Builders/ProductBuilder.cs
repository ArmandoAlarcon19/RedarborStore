using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Tests.Builders;

public class ProductBuilder
{
    private int _id = 1;
    private string _name = "Test Product";
    private string? _description = "Test Description";
    private decimal _price = 99.99m;
    private int _stock = 100;
    private int _categoryId = 1;
    private DateTime _createdDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private DateTime? _updatedDate;

    public ProductBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithStock(int stock)
    {
        _stock = stock;
        return this;
    }

    public ProductBuilder WithCategoryId(int categoryId)
    {
        _categoryId = categoryId;
        return this;
    }
    public ProductBuilder WithUpdatedDate(DateTime updatedDate)
    {
        _updatedDate = updatedDate;
        return this;
    }

    public Product Build()
    {
        return new Product
        {
            Id = _id,
            Name = _name,
            Description = _description,
            Price = _price,
            Stock = _stock,
            CategoryId = _categoryId,
            CreatedDate = _createdDate,
            UpdatedDate = _updatedDate
        };
    }
}