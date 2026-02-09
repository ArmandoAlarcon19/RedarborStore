using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Tests.Builders;

public class CategoryBuilder
{
    private int _id = 1;
    private string _name = "Test Category";
    private string? _description = "Test Description";
    private DateTime _createdDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public CategoryBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CategoryBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CategoryBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public CategoryBuilder WithCreatedDate(DateTime createdDate)
    {
        _createdDate = createdDate;
        return this;
    }

    public Category Build()
    {
        return new Category
        {
            Id = _id,
            Name = _name,
            Description = _description,
            CreatedDate = _createdDate
        };
    }
}