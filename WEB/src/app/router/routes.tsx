import { createBrowserRouter } from "react-router";
import RegisterUserForm from "../../features/auth/RegisterUserForm";
import NotFound from "../../features/errors/NotFound";
import LandingPage from "../LandingPage";
import App from "../App";
import RequireAuth from "./RequireAuth";
import HomePage from "../../features/home/HomePage";

export const routes = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { index: true, element: <LandingPage /> },
      {element:<RequireAuth/>,
         children:
          [{ path: "home", element: <HomePage /> }],
        }
      ,{ path: "register", element: <RegisterUserForm /> },
      { path: "login", element: <LandingPage /> },
      { path: "*", element: <NotFound /> },
    ],
  },
]);
