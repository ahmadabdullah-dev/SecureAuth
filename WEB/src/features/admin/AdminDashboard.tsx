import { useState } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Button,
  Stack,
  Typography,
  CircularProgress,
  Pagination,
  Chip,
} from "@mui/material";
import type { PaginationParams } from "../../lib/types/common";
import { useAdmin } from "../../lib/hooks/useAdmin";


export default function AdminDashboard() {
  const [pagination, setPagination] = useState<PaginationParams>({
    page: 1,
    pageSize: 10,
  });

  const {
    GetUsersAsync,
    DeleteUserAsync,
    AssignToAdminRoleAsync,
    AssignToMemberRoleAsync,
  } = useAdmin(pagination);

  const { data, isLoading } = GetUsersAsync;

  if (isLoading) return <CircularProgress />;

  const users = data?.value?.items ?? [];
  const totalPages = data?.value?.totalPages ?? 1;

  return (
    <Stack>
      <Typography variant="h5">Admin dashboard</Typography>

      <div style={{ overflowX: "auto" }}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Username</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Role</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {users.map((user) => (
              <TableRow key={user.userName}>
                <TableCell>{user.userName}</TableCell>
                <TableCell>{user.email}</TableCell>
                <TableCell>
                  <Chip label={user.role} size="small" sx={{ width: 90 }} />
                </TableCell>
                <TableCell align="right">
                  <Stack>
                    {user.role === "Member" ? (
                      <Button
                        size="small"
                        
                        onClick={() =>
                          AssignToAdminRoleAsync.mutate({
                            userName: user.userName,
                          })
                        }
                      >
                        Make admin
                      </Button>
                    ) : (
                      <Button
                        size="small"
                        onClick={() =>
                          AssignToMemberRoleAsync.mutate({
                            userName: user.userName,
                          })
                        }
                      >
                        Make member
                      </Button>
                    )}
                    <Button
                      size="small"
                      color="error"
                      onClick={() =>
                        DeleteUserAsync.mutate({ userName: user.userName })
                      }
                    >
                      Delete
                    </Button>
                  </Stack>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>

      <Pagination
        page={pagination.page}
        onChange={(_, page) => setPagination((p) => ({ ...p, page }))}
        count={totalPages}
        sx={{ alignSelf: "center" }}
      />
    </Stack>
  );
}
