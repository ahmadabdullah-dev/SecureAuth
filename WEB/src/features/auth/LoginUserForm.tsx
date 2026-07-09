import { useForm } from "react-hook-form";
import { useAuth } from "../../lib/hooks/useAuth";
import type { LoginUserDto } from "../../lib/types/auth";
import { useState } from "react";
import {
  Box,
  Container,
  Paper,
  Stack,
  TextField,
  Typography,
  InputAdornment,
  IconButton,
  CircularProgress,
  Button,
  Alert,
  FormControlLabel,
  Checkbox,
  Link
} from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { useNavigate } from "react-router";

export default function LoginUserForm() {
  const { loginUserAsync } = useAuth();
  const {
    register,
    handleSubmit,
    reset,
    resetField,
    formState: { errors },
  } = useForm<LoginUserDto>({
    defaultValues: { email: "", password: "", isPersistence: false },
  });
  const [showPassword, setShowPassword] = useState(false);

  const onSubmit = (creds: LoginUserDto) => {
    loginUserAsync.mutateAsync(creds, {
      onSuccess: () => {
        reset();
      },
      onError: () => {
        resetField("password");
      },
    });
  };
  const navigate = useNavigate();
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
            Login
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
              <TextField
                label="Password"
                type={showPassword ? "text" : "password"}
                {...register("password", {
                  required: "Password is required",
                  minLength: 6,
                })}
                error={!!errors.password}
                helperText={errors.password?.message}
                fullWidth
                slotProps={{
                  input: {
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton
                          onClick={() => setShowPassword(!showPassword)}
                          edge="end"
                        >
                          {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                      </InputAdornment>
                    ),
                  },
                }}
              />
              <FormControlLabel
                control={<Checkbox {...register("isPersistence")} />}
                label="Remember me"
              />
              <Link
                component="button"
                type="button"
                variant="body2"
                onClick={() => navigate("/forgot-password")}
              >
                Forgot password?
              </Link>
              <Button
                type="submit"
                variant="contained"
                fullWidth
                disabled={loginUserAsync.isPending}
              >
                {loginUserAsync.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : (
                  "Login"
                )}
              </Button>
              {loginUserAsync.data?.isSuccess && (
                <Alert severity="success">{loginUserAsync.data.value}</Alert>
              )}
              {loginUserAsync.error && (
                <Alert severity="error">{loginUserAsync.error.message}</Alert>
              )}
              <Button
                variant="outlined"
                fullWidth
                onClick={() => navigate("/register")}
              >
                Register new user
              </Button>
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
