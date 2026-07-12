import { useForm } from "react-hook-form";
import { useAuth } from "../../lib/hooks/useAuth";
import type { ConfirmEmailDto } from "../../lib/types/auth";
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
} from "@mui/material";

export default function ConfirmEmailForm() {
  const { confirmEmailAsync } = useAuth();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ConfirmEmailDto>({
    defaultValues: { code: "" },
  });

  const onSubmit = (creds: ConfirmEmailDto) => {
    confirmEmailAsync.mutateAsync(creds, {
      onSuccess: () => {
        reset();
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
            Email Confirmation
          </Typography>

          <Box component="form" onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={2}>
              <TextField
                label="Code"
                type="text"
                fullWidth
                {...register("code", { required: "Code is required" })}
                error={!!errors.code}
                helperText={errors.code?.message}
              />
              <Button
                type="submit"
                variant="contained"
                fullWidth
                disabled={confirmEmailAsync.isPending}
              >
                {confirmEmailAsync.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : (
                  "Confirm"
                )}
              </Button>
              {confirmEmailAsync.data?.isSuccess && (
                <Alert severity="success">{confirmEmailAsync.data.value}</Alert>
              )}
              {confirmEmailAsync.error && (
                <Alert severity="error">
                  {confirmEmailAsync.error.message}
                </Alert>
              )}
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
