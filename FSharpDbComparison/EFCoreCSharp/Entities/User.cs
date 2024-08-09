namespace EFCoreCSharp.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<UserCourse> UserCourses { get; set; } = default!;
}
