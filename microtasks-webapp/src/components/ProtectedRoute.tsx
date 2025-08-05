import { Link } from "react-router-dom";
import { useAuth } from "../auth/useAuth";
import { type PropsWithChildren } from "react";

export const ProtectedRoute = ({
  children,
  resource,
  roles,
}: PropsWithChildren<{ resource: string; roles: string[] }>) => {
  const { isAuthenticated, isInRoles } = useAuth();
  return isAuthenticated && isInRoles(resource, roles) ? (
    children
  ) : (
    <>
      <p>You do not have access to this page</p>
      <Link to="/">Go to Home</Link>
    </>
  );
};
