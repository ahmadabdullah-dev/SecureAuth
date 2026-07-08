import { useMutation } from "@tanstack/react-query";
import agent from "../api/agent";
import type {
  LoginUserDto,
  RegisterUserDto,
} from "../types/auth";

export const useAuth = () => {
  
  const registerUserAsync = useMutation({
    mutationFn: async (creds: RegisterUserDto) => {
      const response = await agent.post("/auth/register-user", creds);
      return response.data;
    },
  });
  const loginUserAsync = useMutation({
    mutationFn: async (creds: LoginUserDto) => {
      const response = await agent.post("/auth/login-user", creds);
      return response.data;
    },
  });
  const logoutUserAsync = useMutation({
    mutationFn: async () => {
      const response = await agent.post("/auth/logout-user");
      return response.data;
    },
  });
  return {
    registerUserAsync,
    loginUserAsync,
    logoutUserAsync
  };
};
