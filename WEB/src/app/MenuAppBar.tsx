import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import AccountCircle from "@mui/icons-material/AccountCircle";
import MenuItem from "@mui/material/MenuItem";
import Menu from "@mui/material/Menu";
import LogoutUserButton from "../features/auth/LogoutUserButton";
import { useNavigate } from "react-router";
import { useUser } from "../lib/hooks/useUser";
import { useState, type MouseEvent } from "react";
import {ListItemIcon, ListItemText } from "@mui/material";
import PersonIcon from "@mui/icons-material/Person";
import SettingsIcon from "@mui/icons-material/Settings";
export default function MenuAppBar() {
  const navigate = useNavigate();
  const { CurrentUser } = useUser();

  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleMenu = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = (prop?: string) => {
    if (prop) {
      navigate(prop);
    }
    setAnchorEl(null);
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
          >
            <MenuIcon sx={{ color: "secondary.main" }} />
          </IconButton>

          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Secure Auth
          </Typography>
          {CurrentUser.data?.isSuccess && (
            <div>
              <IconButton
                size="large"
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                onClick={handleMenu}
              >
                <AccountCircle sx={{ color: "secondary.main" }} />
              </IconButton>

              <Menu
                id="menu-appbar"
                anchorEl={anchorEl}
                anchorOrigin={{ vertical: "top", horizontal: "right" }}
                keepMounted
                transformOrigin={{ vertical: "top", horizontal: "right" }}
                open={Boolean(anchorEl)}
                onClose={() => handleClose()}
              >
                <MenuItem
                  sx={{ color: "secondary.main" }}
                  onClick={() => handleClose("/profile")}
                >
                  <ListItemIcon sx={{ color: "inherit" }}>
                    <PersonIcon />
                  </ListItemIcon>
                  <ListItemText>Profile</ListItemText>
                </MenuItem>
                <MenuItem
                  sx={{ color: "secondary.main" }}
                  onClick={() => handleClose("/settings")}
                >
                  <ListItemIcon sx={{ color: "inherit" }}>
                    <SettingsIcon fontSize="small" />
                  </ListItemIcon>
                  <ListItemText>Settings</ListItemText>
                </MenuItem>
                <MenuItem onClick={() => handleClose()}>
                  <LogoutUserButton />
                </MenuItem>
              </Menu>
            </div>
          )}
        </Toolbar>
      </AppBar>
    </Box>
  );
}
