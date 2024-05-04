namespace TangoSchool.ApplicationServices.Models.Classrooms;

public record GetAllClassroomsResponse
(
    List<GetAllClassroomsResponseItem> Items,
    int TotalCount
);
