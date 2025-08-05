import Keycloak from "keycloak-js";
import { type PropsWithChildren } from "react";
import { KeycloakContext } from "./KeycloakContext";

const KeycloakProvider = ({
  children,
  keycloak,
}: PropsWithChildren<{ keycloak: Keycloak }>) => {
  return (
    <KeycloakContext.Provider value={keycloak}>
      {children}
    </KeycloakContext.Provider>
  );
};

export default KeycloakProvider;
