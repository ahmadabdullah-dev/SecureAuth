import { Box, Container, Typography } from "@mui/material";
import LoginUserForm from "../features/auth/LoginUserForm";


export default function LandingPage() {
  return (
    <Box
      sx={{
        minHeight: "100vh",
        bgcolor: "background.default",
        display: "flex",
        alignItems: "center",
        py: { xs: 6, md: 0 },
      }}
    >
      <Container maxWidth="lg">
            <Typography
              variant="h3"
              sx={{
                textAlign: { xs: "center" },
                fontWeight: 700,
                color: "text.primary",
                mb: 5,
              }}
            >
              Secure Auth
            </Typography>
            <LoginUserForm />
      </Container>
    </Box>
  );
}
