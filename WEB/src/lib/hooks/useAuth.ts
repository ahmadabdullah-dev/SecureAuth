import { useMutation } from "@tanstack/react-query";
import agent from "../api/agent";
import type {
  RegisterUserDto,
} from "../types/auth";

export const useAuth = () => {
  
  const registerUserAsync = useMutation({
    mutationFn: async (creds: RegisterUserDto) => {
      const response = await agent.post("/auth/register-user", creds);
      return response.data;
    },
  });

  return {
    registerUserAsync,
  };
};
