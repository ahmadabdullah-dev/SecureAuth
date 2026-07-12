import { useEffect, useState } from "react";
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

const RESEND_COOLDOWN_SECONDS = 30;

export default function ConfirmEmailForm() {
  const { confirmEmailAsync, resendEmailConfirmationCodeAsync } = useAuth();
  const [cooldown, setCooldown] = useState(0);
  const [lastAction, setLastAction] = useState<"confirm" | "resend" | null>(
    null,
  );

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ConfirmEmailDto>({
    defaultValues: { code: "" },
  });

  useEffect(() => {
    if (cooldown <= 0) return;
    const interval = setInterval(() => {
      setCooldown((prev) => (prev <= 1 ? 0 : prev - 1));
    }, 1000);
    return () => clearInterval(interval);
  }, [cooldown]);

  const onSubmit = (creds: ConfirmEmailDto) => {
    confirmEmailAsync
      .mutateAsync(creds, {
        onSuccess: () => {
          reset();
        },
      })
      .then(() => setLastAction("confirm"))
      .catch(() => setLastAction("confirm"));
  };

  const handleResend = () => {
    if (cooldown > 0 || resendEmailConfirmationCodeAsync.isPending) return;
    resendEmailConfirmationCodeAsync
      .mutateAsync()
      .then(() => {
        setCooldown(RESEND_COOLDOWN_SECONDS);
        setLastAction("resend");
      })
      .catch(() => setLastAction("resend"));
  };

  const resendDisabled =
    cooldown > 0 || resendEmailConfirmationCodeAsync.isPending;

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
                inputMode="numeric"
                autoComplete="one-time-code"
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

              <Button
                variant="outlined"
                fullWidth
                disabled={resendDisabled}
                onClick={handleResend}
              >
                {resendEmailConfirmationCodeAsync.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : cooldown > 0 ? (
                  `Resend Confirmation Code (${cooldown}s)`
                ) : (
                  "Resend Confirmation Code"
                )}
              </Button>

              {lastAction === "confirm" &&
                confirmEmailAsync.data?.isSuccess && (
                  <Alert severity="success">
                    {confirmEmailAsync.data.value}
                  </Alert>
                )}
              {lastAction === "confirm" && confirmEmailAsync.error && (
                <Alert severity="error">
                  {confirmEmailAsync.error.message}
                </Alert>
              )}

              {lastAction === "resend" &&
                resendEmailConfirmationCodeAsync.isSuccess && (
                  <Alert severity="success">
                    A new code has been sent to your email.
                  </Alert>
                )}
              {lastAction === "resend" &&
                resendEmailConfirmationCodeAsync.error && (
                  <Alert severity="error">
                    {resendEmailConfirmationCodeAsync.error.message}
                  </Alert>
                )}
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
