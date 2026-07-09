import { useMutation,useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import type {
  LoginUserDto,
  RegisterUserDto,
} from "../types/auth";
import { useNavigate } from "react-router";

export const useAuth = () => {
 
  const queryClient = useQueryClient();
  const navigate = useNavigate();

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
    },  onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["currentUser"] });
      navigate("/home");
    },
  });

  const logoutUserAsync = useMutation({
    mutationFn: async () => {
       await agent.post("/auth/logout-user");
    }, onSuccess: () => {
      queryClient.removeQueries({ queryKey: ["currentUser"] });
      navigate("/login");
    },
  });
  return {
    registerUserAsync,
    loginUserAsync,
    logoutUserAsync
  };
};
