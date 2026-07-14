import { useState } from "react";
import { CircularProgress, Stack } from "@mui/material";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Alert from "@mui/material/Alert";
import { useForm } from "react-hook-form";
import { useUser } from "../../../lib/hooks/useUser";
import type { UpdateCurrentUserDto } from "../../../lib/types/user";

type Message = { severity: "success" | "error"; text: string } | null;

export default function UpdateCurrentUser() {
  const { UpdateCurrentUser } = useUser();
  const [message, setMessage] = useState<Message>(null);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<UpdateCurrentUserDto>({
    defaultValues: {
      firstName: "",
      lastName: "",
      phoneNumber: "",
      country: "",
      dateOfBirth: "",
    },
  });

  const onSubmit = (creds: UpdateCurrentUserDto) => {
    // Only send fields that were actually filled in
    const filledFields = Object.fromEntries(
      Object.entries(creds).filter(
        ([, value]) => value !== "" && value != null,
      ),
    ) as Partial<UpdateCurrentUserDto>;

    if (Object.keys(filledFields).length === 0) {
      setMessage({
        severity: "error",
        text: "Fill at least one field to update.",
      });
      return;
    }

    UpdateCurrentUser.mutateAsync(filledFields, {
      onSuccess: (data) => {
        reset();
        setMessage({
          severity: "success",
          text: data?.value ?? "Profile updated successfully.",
        });
      },
      onError: (err: any) => {
        setMessage({
          severity: "error",
          text: err?.message ?? "Something went wrong.",
        });
      },
    });
  };

  return (
    <Stack
      component="form"
      onSubmit={handleSubmit(onSubmit)}
      spacing={2}
      sx={{
        width: "100%",
        maxWidth: 400,
        mx: "auto",
        p: { xs: 2, sm: 3 },
      }}
    >
      <TextField
        label="First Name"
        fullWidth
        size="small"
        {...register("firstName")}
        error={!!errors.firstName}
        helperText={errors.firstName?.message}
      />

      <TextField
        label="Last Name"
        fullWidth
        size="small"
        {...register("lastName")}
        error={!!errors.lastName}
        helperText={errors.lastName?.message}
      />

      <TextField
        label="Phone Number"
        fullWidth
        size="small"
        {...register("phoneNumber")}
        error={!!errors.phoneNumber}
        helperText={errors.phoneNumber?.message}
      />

      <TextField
        label="Country"
        fullWidth
        size="small"
        {...register("country")}
        error={!!errors.country}
        helperText={errors.country?.message}
      />

      <TextField
        label="Date of Birth"
        type="date"
        fullWidth
        size="small"
        slotProps={{ inputLabel: { shrink: true } }}
        {...register("dateOfBirth")}
        error={!!errors.dateOfBirth}
        helperText={errors.dateOfBirth?.message}
      />

      <Button
        type="submit"
        variant="contained"
        fullWidth
        disabled={UpdateCurrentUser.isPending}
      >
        {UpdateCurrentUser.isPending ? (
          <CircularProgress size={22} color="inherit" />
        ) : (
          "Update Profile"
        )}
      </Button>

      {message && <Alert severity={message.severity}>{message.text}</Alert>}
    </Stack>
  );
}
