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

type ServiceResult<T> =
  | { success: true; data: T }
  | { success: false; error: string };

export const useCompanyService = () => {
  const { token } = useAuth();

  const fetchCompanies = useCallback(async (): Promise<
    ServiceResult<Company[]>
  > => {
    const response = await fetch("/companies", {
      headers: token ? { Authorization: `Bearer ${token}` } : undefined,
    });
    if (!response.ok) {
      const errorText = await response.text();
      return { success: false, error: errorText };
    }
    const companies = await response.json();
    return {
      success: true,
      data: companies.map((c: Company) => ({
        ...c,
        createdAt: new Date(c.createdAt),
        updatedAt: new Date(c.updatedAt),
      })),
    };
  }, [token]);

  const getCompanyById = useCallback(
    async (id: string): Promise<ServiceResult<Company>> => {
      const response = await fetch(`/companies/${id}`, {
        headers: token ? { Authorization: `Bearer ${token}` } : undefined,
      });
      if (!response.ok) {
        const errorText = await response.text();
        return { success: false, error: errorText };
      }
      const c = await response.json();
      return {
        success: true,
        data: {
          ...c,
          createdAt: new Date(c.createdAt),
          updatedAt: new Date(c.updatedAt),
        },
      };
    },
    [token]
  );

  const createCompany = useCallback(
    async (dto: CompanyDto): Promise<ServiceResult<Company>> => {
      const response = await fetch("/companies", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          ...(token ? { Authorization: `Bearer ${token}` } : {}),
        },
        body: JSON.stringify(dto),
      });
      if (!response.ok) {
        const errorText = await response.text();
        return { success: false, error: errorText };
      }
      const c = await response.json();
      return {
        success: true,
        data: {
          ...c,
          createdAt: new Date(c.createdAt),
          updatedAt: new Date(c.updatedAt),
        },
      };
    },
    [token]
  );

  const updateCompany = useCallback(
    async (id: string, dto: CompanyDto): Promise<ServiceResult<Company>> => {
      const response = await fetch(`/companies/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          ...(token ? { Authorization: `Bearer ${token}` } : {}),
        },
        body: JSON.stringify(dto),
      });
      if (!response.ok) {
        const errorText = await response.text();
        return { success: false, error: errorText };
      }
      const c = await response.json();
      return {
        success: true,
        data: {
          ...c,
          createdAt: new Date(c.createdAt),
          updatedAt: new Date(c.updatedAt),
        },
      };
    },
    [token]
  );

  const deleteCompany = useCallback(
    async (id: string): Promise<ServiceResult<null>> => {
      const response = await fetch(`/companies/${id}`, {
        method: "DELETE",
        headers: token ? { Authorization: `Bearer ${token}` } : undefined,
      });
      if (!response.ok) {
        const errorText = await response.text();
        return { success: false, error: errorText };
      }
      return { success: true, data: null };
    },
    [token]
  );

  return {
    fetchCompanies,
    getCompanyById,
    createCompany,
    updateCompany,
    deleteCompany,
  };
};
