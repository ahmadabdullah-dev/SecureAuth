import { useMutation, useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import type {Result} from "../types/common";
import type { RequestUpdateEmailDto, UpdateCurrentUserDto, UpdateEmailDto, updateUserNameDto, UserDto } from "../types/user";
export const useUser = () =>
{
    const CurrentUser = useQuery({
      queryKey: ["currentUser"],
      queryFn: () => agent.get<Result<UserDto>>("/user/current-user").then((res) => res.data),
      staleTime: 5 * 60 * 1000, // cache for 5 min
      retry: false, 
    });

    const RequestUpdateEmail = useMutation({
        mutationFn: async (creds: RequestUpdateEmailDto) => {
         const response = await agent.post("/user/request-update-email",creds);
         return response.data;
        },
    });
    const UpdateEmail = useMutation({
        mutationFn: async (creds: UpdateEmailDto) => {
         const response = await agent.patch("/user/update-email",creds);
         return response.data;
        },
    });
    const ResendUpdateEmailConfirmationCode = useMutation({
        mutationFn: async () => {
         const response = await agent.post("/user/resend-update-email-confirmation-code");
         return response.data;
        },
    });
     const UpdateUserName = useMutation({
       mutationFn: async (creds: updateUserNameDto) => {
         const response = await agent.patch("/user/update-current-username", creds);
         return response.data;
       },
    });
     const UpdateCurrentUser = useMutation({
       mutationFn: async (creds: UpdateCurrentUserDto) => {
         const response = await agent.put("/user/update-current-user",creds);
         return response.data;
       },
     });
     const DeleteCurrentUser = useMutation({
        mutationFn: async () => {
         const response = await agent.delete("/user/delete-current-user");
         return response.data;
        },
    });

    return {
        CurrentUser,
        RequestUpdateEmail,
        UpdateEmail,
        ResendUpdateEmailConfirmationCode,
        UpdateUserName,
        DeleteCurrentUser,
        UpdateCurrentUser
    }
}