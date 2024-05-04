namespace TangoSchool.ApplicationServices.Models.Teachers;

public record GetCurrentTeacherGroupsResponse
(
    List<GetCurrentTeacherGroupsResponseItems> Items,
    int TotalCount
);
