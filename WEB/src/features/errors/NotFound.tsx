import { Box, Container, Paper, Typography } from "@mui/material";

export default function NotFound() {

  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          minHeight: "100vh",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <Paper
          elevation={3}
          sx={{
            p: 4,
            width: "100%",
            textAlign: "center",
          }}
        >
          <Typography variant="h2" sx={{ fontWeight: 700, mb: 1 }}>
            404
          </Typography>
          <Typography variant="h6" sx={{ mb: 1 }}>
            Page not found
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
            This is not the web page you are looking for.
          </Typography>

        </Paper>
      </Box>
    </Container>
  );
}
