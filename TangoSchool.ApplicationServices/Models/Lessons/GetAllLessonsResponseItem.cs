using TangoSchool.ApplicationServices.Models.Classrooms;
using TangoSchool.ApplicationServices.Models.Teachers;

namespace TangoSchool.ApplicationServices.Models.Lessons;

public record GetAllLessonsResponseItem
(
    LessonHeader Lesson,
    TeacherHeader Teacher,
    ClassroomHeader Classroom
);
