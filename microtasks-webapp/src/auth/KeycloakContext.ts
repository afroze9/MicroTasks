import { createContext, useContext } from "react";
import Keycloak from "keycloak-js";

export const KeycloakContext = createContext<Keycloak>({} as Keycloak);
export const useKeycloak = () => useContext(KeycloakContext);
