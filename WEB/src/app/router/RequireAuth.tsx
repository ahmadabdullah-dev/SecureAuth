import { Navigate, Outlet, useLocation } from "react-router";
import {useUser} from "../../lib/hooks/useUser";

export default function RequireAuth() {
  
  const {CurrentUser} = useUser();
  const location = useLocation();
  
  if(CurrentUser.isLoading)
     return <div>Loading...</div>;
  
  if (!CurrentUser.data?.value) 
     return <Navigate to="/login" state={{ from: location }} />;

  return <Outlet />;
}
