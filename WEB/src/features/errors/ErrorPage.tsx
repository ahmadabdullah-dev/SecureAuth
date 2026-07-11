import { useRouteError, Link } from "react-router";

export default function ErrorPage() {
  const error = useRouteError() as { message?: string; statusText?: string };

  return (
    <div style={{ padding: 40, textAlign: "center" }}>
      <h1>Something went wrong</h1>
      <p>{error?.statusText || error?.message || "Unexpected error"}</p>
      <Link to="/">Go back home</Link>
    </div>
  );
}
