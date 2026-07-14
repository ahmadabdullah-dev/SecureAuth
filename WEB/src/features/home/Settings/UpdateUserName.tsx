import { CircularProgress, Stack } from "@mui/material";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Alert from "@mui/material/Alert";
import { useForm } from "react-hook-form";
import { useUser } from "../../../lib/hooks/useUser";
import type { updateUserNameDto } from "../../../lib/types/user";

export default function UpdateUserName() {
  const { UpdateUserName } = useUser();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<updateUserNameDto>({
    defaultValues: { newUserName: "" },
  });

  const onSubmit = (creds: updateUserNameDto) => {
    UpdateUserName.mutateAsync(creds, {
      onSuccess: () => {
        reset();
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
        label="Username"
        fullWidth
        size="small"
        {...register("newUserName", { required: "Username is required" })}
        error={!!errors.newUserName}
        helperText={errors.newUserName?.message}
      />

      <Button
        type="submit"
        variant="contained"
        fullWidth
        disabled={UpdateUserName.isPending}
      >
        {UpdateUserName.isPending ? (
          <CircularProgress size={22} color="inherit" />
        ) : (
          "Update Username"
        )}
      </Button>

      {UpdateUserName.data?.isSuccess && (
        <Alert severity="success">{UpdateUserName.data.value}</Alert>
      )}
      {UpdateUserName.error && (
        <Alert severity="error">{UpdateUserName.error.message}</Alert>
      )}
    </Stack>
  );
}
