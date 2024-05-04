using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.Teachers;

namespace TangoSchool.ApplicationServices.Models.Groups;

public record GroupsMetadata
(
    List<TeacherHeader> Teachers,
    List<StudentHeader> Students
);
