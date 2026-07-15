import { useMutation, useQuery } from "@tanstack/react-query";
import type { AssignToAdminRoleDto, AssignToMemberRoleeDto, DeleteUserDto } from "../types/admin";
import agent from "../api/agent";
import type { PaginationParams, Result } from "../types/common";
import type { UserDto } from "../types/user";

export const useAdmin = (pagination?: PaginationParams) => {
  
    const GetUsersAsync = useQuery({
    queryKey: ["users", pagination?.page, pagination?.pageSize],
    queryFn: async () => await agent.get<Result<UserDto>>("/admin/users", { params: pagination }).then((res) => res.data),
    enabled: !!pagination, // only fetch when pagination is provided
    staleTime: 5 * 60 * 1000,
  });

  const DeleteUserAsync = useMutation({
    mutationFn: async (creds: DeleteUserDto) => {
      const response = await agent.post("/admin/delete-user", creds);
      return response.data;
    },
  });
  const AssignToAdminRoleAsync = useMutation({
    mutationFn: async (creds: AssignToAdminRoleDto) => {
      const response = await agent.post("/admin/assign-to-admin-role", creds);
      return response.data;
    },
  });
  const AssignToMemberRoleAsync = useMutation({
    mutationFn: async (creds: AssignToMemberRoleeDto) => {
      const response = await agent.post("/admin/assign-to-member-role", creds);
      return response.data;
    },
  });
  return {
    GetUsersAsync,
    DeleteUserAsync,
    AssignToAdminRoleAsync,
    AssignToMemberRoleAsync,
  };
};