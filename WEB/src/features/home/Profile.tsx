import {
  Avatar,
  Box,
  Card,
  CardContent,
  Chip,
  CircularProgress,
  Divider,
  Grid,
  Stack,
  Typography,
} from "@mui/material";
import { useUser } from "../../lib/hooks/useUser";

function Field({ label, value }: { label: string; value?: string | null }) {
  return (
    <Box>
      <Typography variant="caption" color="text.secondary">
        {label}
      </Typography>
      <Typography variant="body1">{value || "-"}</Typography>
    </Box>
  );
}

export default function Profile() {
  const { CurrentUser } = useUser();
  const user = CurrentUser.data?.value;

  const joinedDateOnly = user?.joinedDate
    ? new Date(user.joinedDate).toLocaleDateString()
    : "-";

  const birthDateOnly = user?.birthDate
    ? new Date(user.birthDate).toLocaleDateString()
    : "-";

  const initials = [user?.firstName?.[0], user?.lastName?.[0]]
    .filter(Boolean)
    .join("")
    .toUpperCase();

  if (CurrentUser.isPending) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", py: 8 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (CurrentUser.isError) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", py: 8 }}>
        <Typography color="error">Failed to load profile.</Typography>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        py: 5,
        px: 2,
      }}
    >
      <Card variant="outlined" sx={{ maxWidth: 700, width: "100%" }}>
        <CardContent sx={{ p: { xs: 2, sm: 4 } }}>
          <Stack
            direction="row"
            spacing={2}
            
            sx={{
              flexWrap:"wrap",
              alignItems: "center",
              justifyContent: "space-between",
            }}
          >
            <Stack direction="row" spacing={2} sx={{alignItems:"center"}} >
              <Avatar sx={{ width: 64, height: 64, fontSize: 24 }}>
                {initials || "?"}
              </Avatar>
              <Box>
                <Typography variant="h5" sx={{fontWeight:"600"}}>
                  {user?.firstName} {user?.lastName}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  @{user?.userName}
                </Typography>
              </Box>
            </Stack>

            <Chip
              label={
                user?.emailConfirmed ? "Email verified" : "Email not verified"
              }
              color={user?.emailConfirmed ? "success" : "warning"}
              size="small"
              variant="outlined"
            />
          </Stack>

          <Divider sx={{ my: 3 }} />

          <Grid container spacing={3}>
            <Grid size={{ xs: 12, sm: 6 }}>
              <Field label="Email" value={user?.email} />
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Field label="Phone" value={user?.phoneNumber} />
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Field label="Country" value={user?.country} />
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Field label="Date of birth" value={birthDateOnly} />
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Field label="Role" value={user?.role} />
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Field label="Joined" value={joinedDateOnly} />
            </Grid>
          </Grid>
        </CardContent>
      </Card>
    </Box>
  );
}
