import { useState } from "react";
import { useAuth } from "../../lib/hooks/useAuth";
import type { RegisterUserDto } from "../../lib/types/auth";
import { Container,CircularProgress,Alert, Box, Paper, Typography, TextField, InputAdornment, IconButton, Button, Stack } from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { useForm } from "react-hook-form";

export default function RegisterUserForm() {
  const { registerUserAsync } = useAuth();
  const {register,handleSubmit, reset,resetField, formState: {errors}} = useForm<RegisterUserDto>({
    defaultValues: {userName: "", email: "", password: ""}
  })
  const [showPassword, setShowPassword] = useState(false);

  const onSubmit = (creds: RegisterUserDto) => {
    registerUserAsync.mutateAsync(creds,{
        onSuccess: () =>{
            reset();
        },
        onError: () => {
            resetField("password")
        }
    });
  };
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
        <Paper sx={{ p: 4, width: "100%" }}>
          <Typography
            variant="h3"
            sx={{
              m: 2,
              textAlign: "center",
            }}
          >
            Register
          </Typography>

          <Box component="form" onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={2}>
              <TextField
                label="UserName"
                fullWidth
                {...register("userName", { required: "Username is required" })}
                error={!!errors.userName}
                helperText={errors.userName?.message}
              />
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
              <Button
                type="submit"
                variant="contained"
                fullWidth
                disabled={registerUserAsync.isPending}
              >
                {registerUserAsync.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : (
                  "Register"
                )}
              </Button>
              {registerUserAsync.data?.isSuccess && (
                <Alert severity="success">{registerUserAsync.data.value}</Alert>
              )}
              {registerUserAsync.error && (
                <Alert severity="error">
                  {registerUserAsync.error.message}
                </Alert>
              )}
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
