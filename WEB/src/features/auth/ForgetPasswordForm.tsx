import { useForm } from "react-hook-form";
import { useAuth } from "../../lib/hooks/useAuth";
import type { ForgetPasswordDto } from "../../lib/types/auth";
import {
  Box,
  Container,
  Paper,
  Stack,
  TextField,
  Typography,
  CircularProgress,
  Button,
  Alert,
  Link
} from "@mui/material";
import { useNavigate } from "react-router";

export default function ForgetPasswordForm() {
  const { forgetPasswordAsync } = useAuth();
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ForgetPasswordDto>({
    defaultValues: { email: "" },
  });

 const onSubmit = (creds: ForgetPasswordDto) => {
   forgetPasswordAsync.mutate(creds, {
     onSuccess: (data) => {
       if (data.isSuccess) {
         navigate(`/reset-password/${encodeURIComponent(creds.email)}`);
       }
     },
   });
 };
 

  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          minHeight: "70vh",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <Paper sx={{ p: 4, width: "100%" }}>
          <Typography variant="h3" sx={{ m: 2, textAlign: "center" }}>
            Find Yourself
          </Typography>

          <Box component="form" onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={2}>
              <TextField
                label="Email"
                type="email"
                fullWidth
                {...register("email", { required: "Email is required" })}
                error={!!errors.email}
                helperText={errors.email?.message}
              />
              <Button
                type="submit"
                variant="contained"
                fullWidth
                disabled={forgetPasswordAsync.isPending}
              >
                {forgetPasswordAsync.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : (
                  "Continue"
                )}
              </Button>
              {forgetPasswordAsync.error && (
                <Alert severity="error">
                  {forgetPasswordAsync.error.message}
                </Alert>
              )}
              <Link
                component="button"
                type="button"
                variant="body2"
                onClick={() => navigate("/login")}
              >
                Back to login
              </Link>
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
