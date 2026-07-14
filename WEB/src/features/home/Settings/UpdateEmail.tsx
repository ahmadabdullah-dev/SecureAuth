import { useState, useEffect } from "react";
import { CircularProgress, Stack} from "@mui/material";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Alert from "@mui/material/Alert";
import { useForm } from "react-hook-form";
import { useUser } from "../../../lib/hooks/useUser";
import type {
  UpdateEmailDto,
  RequestUpdateEmailDto,
} from "../../../lib/types/user";

const RESEND_SECONDS = 30;

type Message = { severity: "success" | "error"; text: string } | null;

export default function UpdateEmail() {
  const { UpdateEmail, RequestUpdateEmail, ResendUpdateEmailConfirmationCode } = useUser();
  const [step, setStep] = useState<"email" | "code">("email");
  const [cooldown, setCooldown] = useState(0);
  const [message, setMessage] = useState<Message>(null);

  useEffect(() => {
    if (cooldown === 0) return;
    const id = setTimeout(() => setCooldown((c) => c - 1), 1000);
    return () => clearTimeout(id);
  }, [cooldown]);

  const emailForm = useForm<RequestUpdateEmailDto>({
    defaultValues: { newEmail: "" },
  });

  const codeForm = useForm<UpdateEmailDto>({
    defaultValues: { code: "" },
  });

  const onSubmitEmail = (creds: RequestUpdateEmailDto) => {
    RequestUpdateEmail.mutateAsync(creds, {
      onSuccess: (data) => {
        setStep("code");
        setCooldown(RESEND_SECONDS);
        setMessage({
          severity: "success",
          text: data?.value ?? "Confirmation code sent.",
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

  const onSubmitCode = (creds: UpdateEmailDto) => {
    UpdateEmail.mutateAsync(creds, {
      onSuccess: (data) => {
        codeForm.reset();
        setMessage({
          severity: "success",
          text: data?.value ?? "Email updated successfully.",
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

  const onResend = () => {
    ResendUpdateEmailConfirmationCode.mutateAsync(undefined, {
      onSuccess: () => {
        setCooldown(RESEND_SECONDS);
        setMessage({ severity: "success", text: "Code resent" });
      },
      onError: (err: any) => {
        setMessage({
          severity: "error",
          text: err?.message ?? "Failed to resend.",
        });
      },
    });
  };

  if (step === "email") {
    return (
      <Stack
        component="form"
        onSubmit={emailForm.handleSubmit(onSubmitEmail)}
        spacing={2}
        sx={{ width: "100%", maxWidth: 400, mx: "auto", p: { xs: 2, sm: 3 } }}
      >
        <TextField
          label="New Email"
          type="email"
          fullWidth
          size="small"
          {...emailForm.register("newEmail", { required: "Email is required" })}
          error={!!emailForm.formState.errors.newEmail}
          helperText={emailForm.formState.errors.newEmail?.message}
        />

        <Button
          type="submit"
          variant="contained"
          fullWidth
          disabled={RequestUpdateEmail.isPending}
        >
          {RequestUpdateEmail.isPending ? (
            <CircularProgress size={22} color="inherit" />
          ) : (
            "Send Confirmation Code"
          )}
        </Button>

        {message && <Alert severity={message.severity}>{message.text}</Alert>}
      </Stack>
    );
  }

  return (
    <Stack
      component="form"
      onSubmit={codeForm.handleSubmit(onSubmitCode)}
      spacing={2}
      sx={{ width: "100%", maxWidth: 400, mx: "auto", p: { xs: 2, sm: 3 } }}
    >
      <TextField
        label="Confirmation Code"
        fullWidth
        size="small"
        {...codeForm.register("code", { required: "Code is required" })}
        error={!!codeForm.formState.errors.code}
        helperText={codeForm.formState.errors.code?.message}
      />

      <Button
        type="submit"
        variant="contained"
        fullWidth
        disabled={UpdateEmail.isPending}
      >
        {UpdateEmail.isPending ? (
          <CircularProgress size={22} color="inherit" />
        ) : (
          "Confirm Email"
        )}
      </Button>

      <Button
        variant="text"
        fullWidth
        disabled={ResendUpdateEmailConfirmationCode.isPending || cooldown > 0}
        onClick={onResend}
      >
        {ResendUpdateEmailConfirmationCode.isPending ? (
          <CircularProgress size={20} color="inherit" />
        ) : cooldown > 0 ? (
          `Resend Code (${cooldown}s)`
        ) : (
          "Resend Code"
        )}
      </Button>

      {message && <Alert severity={message.severity}>{message.text}</Alert>}
    </Stack>
  );
}
