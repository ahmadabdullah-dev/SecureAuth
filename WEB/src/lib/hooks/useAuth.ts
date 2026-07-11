import { useMutation,useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import type {
  ConfirmEmailDto,
  ForgetPasswordDto,
  LoginUserDto,
  RegisterUserDto,
  ResetPasswordDto,
} from "../types/auth";
import { useNavigate } from "react-router";

export const useAuth = () => {
 
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const registerUserAsync = useMutation({
    mutationFn: async (creds: RegisterUserDto) => {
      const response = await agent.post("/auth/register", creds);
      return response.data;
    },
  });

  const loginUserAsync = useMutation({
    mutationFn: async (creds: LoginUserDto) => {
      const response = await agent.post("/auth/login", creds);
      return response.data;
    },  onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["currentUser"] });
      navigate("/home");
    },
  });

  const logoutUserAsync = useMutation({
    mutationFn: async () => {
       await agent.post("/auth/logout");
    }, onSuccess: () => {
      queryClient.removeQueries({ queryKey: ["currentUser"] });
      navigate("/login");
    },
  });
  const forgetPasswordAsync = useMutation({
    mutationFn: async (creds: ForgetPasswordDto) => {
      const response = await agent.post("/auth/forget-password", creds);
      return response.data;
    },
  });
 const resetPasswordAsync = useMutation({
   mutationFn: async (creds: ResetPasswordDto) => {
     const response = await agent.post("/auth/reset-password", creds);
     return response.data;
   },
 });
 const resendEmailConfirmationCodeAsync = useMutation({
   mutationFn: async () => {
     const response = await agent.post("/auth/resend-email-confirmation-code");
     return response.data;
   },
 });
const confirmEmailAsync = useMutation({
  mutationFn: async (creds: ConfirmEmailDto) => {
    const response = await agent.post("/auth/confirm-email",creds);
    return response.data;
  },
});

  return {
    registerUserAsync,
    loginUserAsync,
    logoutUserAsync,
    forgetPasswordAsync,
    resetPasswordAsync,
    resendEmailConfirmationCodeAsync,
    confirmEmailAsync
  };
};
