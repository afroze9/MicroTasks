import { useEffect, useState } from "react";
import "./App.css";
import { fetchCompanies } from "./services/companyService";
import type { Company } from "./services/companyService";

function App() {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    fetchCompanies()
      .then(setCompanies)
      .catch((err) => setError(err.message));
  }, []);

  return (
    <>
      <h1>Companies</h1>

      <div className="read-the-docs">
        {error && <p style={{ color: "red" }}>{error}</p>}
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr>
              <th style={{ border: "1px solid #ccc", padding: "8px" }}>Name</th>
              <th style={{ border: "1px solid #ccc", padding: "8px" }}>
                Created At
              </th>
              <th style={{ border: "1px solid #ccc", padding: "8px" }}>
                Updated At
              </th>
              <th style={{ border: "1px solid #ccc", padding: "8px" }}>Tags</th>
            </tr>
          </thead>
          <tbody>
            {companies.map((company) => (
              <tr key={company.id}>
                <td style={{ border: "1px solid #ccc", padding: "8px" }}>
                  {company.name}
                </td>
                <td style={{ border: "1px solid #ccc", padding: "8px" }}>
                  {company.createdAt.toLocaleString()}
                </td>
                <td style={{ border: "1px solid #ccc", padding: "8px" }}>
                  {company.updatedAt.toLocaleString()}
                </td>
                <td style={{ border: "1px solid #ccc", padding: "8px" }}>
                  {company.tags && company.tags.length > 0
                    ? company.tags.map((tag) => tag.value).join(", ")
                    : "-"}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
}

export default App;
