import { Box, Container, Grid, Typography } from "@mui/material";
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
        <Grid container sx={{ alignItems: "center" }}>
          <Grid size={{ xs: 12, md: 7 }}>
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
          </Grid>

          <Grid
            size={{ xs: 12, md: 5 }}
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              justifyContent: "center",
              textAlign: "center",
            }}
          >
            <Typography
              variant="h6"
              sx={{
                color: "text.secondary",
                fontWeight: 400,
                my: 4,
                maxWidth: 480,
              }}
            >
              A full-stack authentication foundation built with React, and
              ASP.NET Web API using ASP.NET Identity. Designed to be reusable
              for next projects.
            </Typography>
          </Grid>
        </Grid>
      </Container>
    </Box>
  );
}
