import { createBrowserRouter } from "react-router";
import RegisterUserForm from "../../features/auth/RegisterUserForm";
import NotFound from "../../features/errors/NotFound";
import LandingPage from "../LandingPage";
import App from "../App";
import RequireAuth from "./RequireAuth";
import HomePage from "../../features/home/HomePage";
import Profile from "../../features/home/Profile";
import Settings from "../../features/home/Settings";

export const routes = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { index: true, element: <LandingPage /> },
      {
        element: <RequireAuth />,
        children: [
          { path: "home", element: <HomePage /> },
          { path: "profile", element: <Profile /> },
          { path: "settings", element: <Settings /> },
        ],
      },
      { path: "register", element: <RegisterUserForm /> },
      { path: "login", element: <LandingPage /> },
      { path: "*", element: <NotFound /> },
    ],
  },
]);
