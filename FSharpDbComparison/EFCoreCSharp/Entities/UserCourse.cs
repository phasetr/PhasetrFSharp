namespace EFCoreCSharp.Entities;

public class UserCourse
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
}
