import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import "./App.css";
import CompaniesPage from "./pages/CompaniesPage";
import KeycloakProvider from "./auth/KeycloakProvider";
import { ProtectedRoute } from "./components/ProtectedRoute";
import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "http://localhost:9173",
  realm: "microtasks",
  clientId: "microtasks-webapp",
});

keycloak
  .init({
    onLoad: "login-required",
    scope: "openid profile email",
  })
  .then((authenticated) => {
    if (authenticated) {
      console.log("User is authenticated");
    } else {
      console.warn("User is not authenticated");
    }
  });

keycloak.onAuthSuccess = () => {
  console.log("Authentication successful ID", keycloak.idTokenParsed);
  console.log("Authentication successful AC", keycloak.tokenParsed);
};
keycloak.onAuthError = (error) => {
  console.error("Authentication error", error);
};

function App() {
  return (
    <KeycloakProvider keycloak={keycloak}>
      <BrowserRouter>
        <nav style={{ marginBottom: "1rem" }}>
          <Link to="/companies">Companies</Link>
        </nav>
        <Routes>
          <Route path="/" element={<div>Welcome to MicroTasks!</div>} />
          <Route
            path="/companies"
            element={
              <ProtectedRoute
                resource="company-api"
                roles={[
                  "company_viewer",
                  "company_manager",
                  "company_contributor",
                ]}
              >
                <CompaniesPage />
              </ProtectedRoute>
            }
          />
        </Routes>
      </BrowserRouter>
    </KeycloakProvider>
  );
}

export default App;
{
  /* <button onClick={() => keycloak.logout()}>Logout</button> */
}
