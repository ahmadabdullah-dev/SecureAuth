import { Outlet } from "react-router";
import Footer from "./Footer";
import MenuAppBar from "./MenuAppBar";
import { Box } from "@mui/material";

function App() {
  return (
    <>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          minHeight: "100vh",
        }}
      >
        <MenuAppBar />
        <Box component="main" sx={{ flexGrow: 1 }}>
          <Outlet />
        </Box>
        <Footer />
      </Box>
    </>
  );
}
export default App;
