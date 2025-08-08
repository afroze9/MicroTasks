import { useAuth } from "../auth/useAuth";
import { useCallback } from "react";
import type { Company, CompanyDto } from "../types/companyTypes";
import type { ServiceResult } from "../types/ServiceResult";

export const useCompanyService = () => {
  const { token } = useAuth();

  const fetchCompanies = useCallback(async (): Promise<
    ServiceResult<Company[]>
  > => {
    const response = await fetch("/api/companies", {
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
      const response = await fetch(`/api/companies/${id}`, {
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
      const response = await fetch("/api/companies", {
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
      const response = await fetch(`/api/companies/${id}`, {
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
      const response = await fetch(`/api/companies/${id}`, {
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
