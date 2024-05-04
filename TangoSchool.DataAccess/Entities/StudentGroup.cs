namespace TangoSchool.DataAccess.Entities;

public class StudentGroup
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public Guid GroupId { get; set; }

    public Group Group { get; set; } = null!;

    public DateTimeOffset JoinDate { get; set; }
}
