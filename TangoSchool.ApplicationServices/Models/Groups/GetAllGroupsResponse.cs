namespace TangoSchool.ApplicationServices.Models.Groups;

public record GetAllGroupsResponse
(
    List<GetAllGroupsResponseItem> Items,
    int TotalCount
);
