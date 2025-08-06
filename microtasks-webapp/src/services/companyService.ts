import { useAuth } from "../auth/useAuth";
import { useCallback } from "react";

export type Tag = {
  id: string; // Guid
  value: string;
};

export type Company = {
  id: string; // Guid
  name: string;
  tags: Tag[];
  createdAt: Date;
  updatedAt: Date;
};

export type TagDto = {
  value: string;
};

export type CompanyDto = {
  name: string;
  tags?: TagDto[];
};

export const useCompanyService = () => {
  const { token } = useAuth();

  const fetchCompanies = useCallback(async (): Promise<Company[]> => {
    const response = await fetch("/companies", {
      headers: token ? { Authorization: `Bearer ${token}` } : undefined,
    });

    if (!response.ok) {
      let errorMessage = `Error: ${response.status}`;
      try {
        const errorBody = await response.json();
        if (errorBody && errorBody.error) {
          errorMessage = `${errorBody.error}${
            errorBody.detail ? ": " + errorBody.detail : ""
          }`;
        }
      } catch {
        // Ignore JSON parse errors, fallback to status
      }

      if (response.status === 401) {
        throw new Error("Unauthorized: Please log in.");
      } else if (response.status === 403) {
        throw new Error("Forbidden: You do not have access to this resource.");
      } else if (response.status === 500) {
        throw new Error("Server error. Please try again later.");
      } else {
        throw new Error(errorMessage);
      }
    }

    const companies = await response.json();
    return companies.map((c: Company) => ({
      ...c,
      createdAt: new Date(c.createdAt),
      updatedAt: new Date(c.updatedAt),
    }));
  }, [token]);

  return { fetchCompanies };
};
