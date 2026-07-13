namespace Business.Dtos;

public record DeleteUserDto(string username);
public record AssignToMemberRoleDto(string username);
public record AssignToAdminRoleDto(string username);