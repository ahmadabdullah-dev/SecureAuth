import { createTheme } from "@mui/material/styles";

export const theme = createTheme({
  palette: {
    primary: {
      main: "#EB5E28", // Spicy Paprika
      dark: "#C74A1C",
      light: "#F08055",
    },
    secondary: {
      main: "#403D39", 
      dark: "#252422", 
      light: "#5C5852",
    },
    background: {
      default: "#F5F3F0", 
      paper: "#FFFFFF",
    },
    error: {
      main: "#DC2626",
    },
    success: {
      main: "#16A34A",
    },
    text: {
      primary: "#252422", 
      secondary: "#403D39", 
    },
  },
  shape: {
    borderRadius: 12,
  },
  typography: {
    fontFamily: `"Inter", "Roboto", "Helvetica", "Arial", sans-serif`,
    h5: { fontWeight: 700 },
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: { textTransform: "none", fontWeight: 600 },
      },
    },
    MuiTextField: {
      defaultProps: {
        variant: "outlined",
      },
    },
  },
});
