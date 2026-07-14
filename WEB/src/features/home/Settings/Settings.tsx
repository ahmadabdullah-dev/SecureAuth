import { useState } from "react";
import {
  Container,
  Box,
  Typography,
  Tabs,
  Tab,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import UpdateEmail from "./UpdateEmail";
import UpdateCurrentUser from "./UpdateUser";
import UpdateUserName from "./UpdateUserName";
import DeleteUser from "./DeleteUser";

const SECTIONS = [
  { label: "Profile", component: <UpdateCurrentUser /> },
  { label: "Username", component: <UpdateUserName /> },
  { label: "Email", component: <UpdateEmail /> },
  { label: "Danger Zone", component: <DeleteUser /> },
] as const;

export default function Settings() {
  const [activeTab, setActiveTab] = useState(0);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  return (
    <Container maxWidth="md" sx={{ py: { xs: 3, sm: 5 } }}>
      <Typography variant="h5" sx={{fontWeight:"600"}} gutterBottom>
        Settings
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
        Manage your account details and preferences.
      </Typography>

      <Tabs
        value={activeTab}
        onChange={(_, value) => setActiveTab(value)}
        variant={isMobile ? "scrollable" : "standard"}
        scrollButtons={isMobile ? "auto" : false}
        sx={{ borderBottom: 1, borderColor: "divider" }}
      >
        {SECTIONS.map((section, index) => (
          <Tab key={section.label} label={section.label} value={index} />
        ))}
      </Tabs>

      <Box sx={{ pt: 3 }}>{SECTIONS[activeTab].component}</Box>
    </Container>
  );
}
