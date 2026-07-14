import { useState } from "react";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
  Typography,
  CircularProgress,
  Alert,
} from "@mui/material";
import { useUser } from "../../../lib/hooks/useUser";

const CONFIRM_TEXT = "DELETE";

export default function DeleteUser() {
  const { DeleteCurrentUser } = useUser();
  const [open, setOpen] = useState(false);
  const [confirmText, setConfirmText] = useState("");
  const [message, setMessage] = useState<string | null>(null);

  const isConfirmed = confirmText === CONFIRM_TEXT;

  const handleClose = () => {
    setOpen(false);
    setConfirmText("");
    setMessage(null);
  };

  const onDelete = () => {
    DeleteCurrentUser.mutateAsync(undefined, {
      onSuccess: () => {
        handleClose();
      },
      onError: (err: any) => {
        setMessage(err?.message ?? "Failed to delete account.");
      },
    });
  };

  return (
    <Box
      sx={{
        width: "100%",
        maxWidth: 400,
        mx: "auto",
        p: { xs: 2, sm: 3 },
      }}
    >
      <Typography
        variant="subtitle1"
        sx={{fontWeight:"600"}}
        color="error"
        gutterBottom
      >
        Delete User
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
        This will permanently delete yourself and all associated data. This
        action cannot be undone.
      </Typography>

      <Button
        variant="outlined"
        color="error"
        fullWidth
        onClick={() => setOpen(true)}
      >
        Delete Me
      </Button>

      <Dialog open={open} onClose={handleClose} maxWidth="xs" fullWidth>
        <DialogTitle>Confirm account deletion</DialogTitle>
        <DialogContent>
          <DialogContentText sx={{ mb: 2 }}>
            This action is permanent. Type <strong>{CONFIRM_TEXT}</strong> below
            to confirm.
          </DialogContentText>

          <TextField
            fullWidth
            size="small"
            autoFocus
            placeholder={CONFIRM_TEXT}
            value={confirmText}
            onChange={(e) => setConfirmText(e.target.value)}
          />

          {message && (
            <Alert severity="error" sx={{ mt: 2 }}>
              {message}
            </Alert>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} disabled={DeleteCurrentUser.isPending}>
            Cancel
          </Button>
          <Button
            color="error"
            variant="contained"
            disabled={!isConfirmed || DeleteCurrentUser.isPending}
            onClick={onDelete}
          >
            {DeleteCurrentUser.isPending ? (
              <CircularProgress size={20} color="inherit" />
            ) : (
              "Delete User"
            )}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
