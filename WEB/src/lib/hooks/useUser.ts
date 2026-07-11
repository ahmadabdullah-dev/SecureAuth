import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import type {Result} from "../types/common";
import type { UserDto } from "../types/user";
export const useUser = () =>
{
    const CurrentUser = useQuery({
      queryKey: ["currentUser"],
      queryFn: () => agent.get<Result<UserDto>>("/user/current-user").then((res) => res.data),
      staleTime: 5 * 60 * 1000, // cache for 5 min
      retry: false, 
    });

    return {
        CurrentUser
    }
}