import { useKeycloak } from "./KeycloakContext";

export const useAuth = () => {
  const keycloak = useKeycloak();

  return {
    isAuthenticated: !!keycloak?.token,
    token: keycloak?.token,
    login: keycloak?.login,
    logout: keycloak?.logout,
    userInfo: keycloak?.tokenParsed,
    hasRole: (resource: string, role: string) => {
      return (
        keycloak?.tokenParsed?.resource_access?.[resource]?.roles?.includes(
          role
        ) || false
      );
    },
    isInRoles: (resource: string, roles: string[]) => {
      return roles.some((role) =>
        keycloak?.tokenParsed?.resource_access?.[resource]?.roles?.includes(
          role
        )
      );
    },
  };
};
