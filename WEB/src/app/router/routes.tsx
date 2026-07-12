import { createBrowserRouter, Navigate,Outlet } from "react-router";
import RegisterUserForm from "../../features/auth/RegisterUserForm";
import NotFound from "../../features/errors/NotFound";
import App from "../App";
import RequireAuth from "./RequireAuth";
import HomePage from "../../features/home/HomePage";
import Profile from "../../features/home/Profile";
import Settings from "../../features/home/Settings";
import { useUser } from "../../lib/hooks/useUser";
import LoginUserForm from "../../features/auth/LoginUserForm";
import ErrorPage from "../../features/errors/ErrorPage";
import ForgetPasswordForm from "../../features/auth/ForgetPasswordForm";
import ResetPasswordForm from "../../features/auth/ResetPasswordForm";
import ConfirmEmailForm from "../../features/auth/ConfirmEmailForm";

export default function RedirectIfAuth() {
  const { CurrentUser } = useUser();

  if (CurrentUser.isLoading) return <div>Loading...</div>;

  return CurrentUser.data?.value ? <Navigate to="/home" replace /> : <Outlet />;
}

export const routes = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <ErrorPage />,
    children: [
      { index: true, element: <Navigate to="/login" replace /> },
      {
        element: <RequireAuth />,
        children: [
          { path: "home", element: <HomePage /> },
          { path: "profile", element: <Profile /> },
          { path: "settings", element: <Settings /> },  
          {path: "confirm-email", element: <ConfirmEmailForm/>}

        ],
      },
      {
        element: <RedirectIfAuth />,
        children: [
          { path: "register", element: <RegisterUserForm /> },
          { path: "login", element: <LoginUserForm /> },
          { path: "forget-password", element: <ForgetPasswordForm /> },
          { path: "reset-password/:email", element: <ResetPasswordForm /> } 

        ],
      },
      { path: "*", element: <NotFound /> },
    ],
  },
]);
