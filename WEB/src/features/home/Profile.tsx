import {
  Box,
  Card,
  CardContent,
  Divider,
  Grid,
  Stack,
  Typography,
} from "@mui/material";
import { useUser } from "../../lib/hooks/useUser";

export default function Profile() {
  const { CurrentUser } = useUser();
  const user = CurrentUser.data?.value;

  const joinedDateOnly = user?.joinedDate
    ? new Date(user.joinedDate).toLocaleDateString()
    : "-";

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        py: 5,
        px: 2,
      }}
    >
      <Card sx={{ maxWidth: 700, width: "100%" }}>
        <CardContent>
          <Stack spacing={1} >
            <Box>
              <Typography variant="h5" sx={{fontWeight:"600"}}>
                {user?.firstName} {user?.lastName}
              </Typography>

              <Typography>@{user?.userName}</Typography>
            </Box>
          </Stack>

          <Divider sx={{ my: 3 }} />

          <Grid container spacing={2}>
            <Grid size={{ xs: 12, sm: 6 }}>
              <Typography variant="body2">
                First name
              </Typography>
              <Typography>{user?.firstName || "-"}</Typography>
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Typography variant="body2">
                Last name
              </Typography>
              <Typography>{user?.lastName || "-"}</Typography>
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Typography variant="body2">
                Email
              </Typography>
              <Typography>{user?.email}</Typography>
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Typography variant="body2">
                Phone
              </Typography>
              <Typography>{user?.phoneNumber || "-"}</Typography>
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Typography variant="body2">
                Role
              </Typography>
              <Typography>{user?.role || "-"}</Typography>
            </Grid>

            <Grid size={{ xs: 12, sm: 6 }}>
              <Typography variant="body2" >
                Joined
              </Typography>
              <Typography>{joinedDateOnly}</Typography>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
    </Box>
  );
}
