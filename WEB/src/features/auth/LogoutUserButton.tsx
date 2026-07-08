import { Button } from "@mui/material";
import LogoutIcon from "@mui/icons-material/Logout";
import { useAuth } from "../../lib/hooks/useAuth";

export default function LogoutUserButton() {
  const { logoutUserAsync } = useAuth();

  return (
    <Button
      onClick={() => logoutUserAsync.mutate()}
      disabled={logoutUserAsync.isPending}
      variant="outlined"
      color="error"
      startIcon={<LogoutIcon />}
    >
      {logoutUserAsync.isPending ? "Logging out…" : "Log out"}
    </Button>
  );
}
