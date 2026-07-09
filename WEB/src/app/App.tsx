import { Outlet } from "react-router";
import Footer from "./Footer";
function App() {
  return (
    <>
      <Outlet />
      <Footer/>
    </>
  );
}
export default App;
