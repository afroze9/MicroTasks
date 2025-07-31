import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import "./App.css";
import CompaniesPage from "./pages/CompaniesPage";

function App() {
  return (
    <BrowserRouter>
      <nav style={{ marginBottom: "1rem" }}>
        <Link to="/companies">Companies</Link>
      </nav>
      <Routes>
        <Route path="/companies" element={<CompaniesPage />} />
        <Route path="/" element={<div>Welcome to MicroTasks!</div>} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
