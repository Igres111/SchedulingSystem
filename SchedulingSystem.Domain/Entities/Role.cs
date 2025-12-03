using SchedulingSystem.Domain.BaseTypes;

namespace SchedulingSystem.Domain.Entities;

public class Role : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; 

    public ICollection<User> Users { get; set; } = new List<User>();
}