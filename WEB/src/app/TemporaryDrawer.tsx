import * as React from "react";
import Box from "@mui/material/Box";
import Drawer from "@mui/material/Drawer";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import InboxIcon from "@mui/icons-material/MoveToInbox";
import MailIcon from "@mui/icons-material/Mail";
import { Link } from "react-router";

interface TemporaryDrawerProps {
  open: boolean;
  onClose: () => void;
}

interface DrawerItem {
  text: string;
  icon: React.ReactNode;
  path: string;
}

const items: DrawerItem[] = [
  { text: "Inbox", icon: <InboxIcon />, path: "/inbox" },
  { text: "Starred", icon: <MailIcon />, path: "/starred" },
  { text: "Send email", icon: <InboxIcon />, path: "/send" },
  { text: "Drafts", icon: <MailIcon />, path: "/drafts" },
];


export default function TemporaryDrawer({
  open,
  onClose,
}: TemporaryDrawerProps) {
  const renderItems = (items: DrawerItem[]) => (
    <List sx={{ py: 1 }}>
      {items.map(({ text, icon, path }) => (
        <ListItem key={text} disablePadding sx={{ mb: 0.5 }}>
          <ListItemButton component={Link} to={path} sx={{ borderRadius: 2 }}>
            <ListItemIcon sx={{color:"primary.main", minWidth: 40 }}>{icon}</ListItemIcon>
            <ListItemText primary={text} />
          </ListItemButton> 
        </ListItem>
      ))}
    </List>
  );

  return (
    <Drawer open={open} onClose={onClose} >
      <Box sx={{ width: 260, p: 1, }} role="presentation" onClick={onClose}>
        {renderItems(items)}
       
      </Box>
    </Drawer>
  );
}
