using TangoSchool.ApplicationServices.Models.Classrooms;
using TangoSchool.ApplicationServices.Models.Groups;
using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.Teachers;

namespace TangoSchool.ApplicationServices.Models.Lessons;

public record LessonsMetadata
(
    List<TeacherHeader> Teachers,
    List<StudentHeader> Students,
    List<ClassroomHeader> Classrooms,
    List<GroupHeader> Groups
);
