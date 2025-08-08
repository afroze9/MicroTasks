import { useAuth } from "../auth/useAuth";
import { useCallback } from "react";
import type { Project, ProjectDto } from "../types/projectTypes";
import type { ServiceResult } from "../types/ServiceResult";

export const useProjectService = () => {
  const { token } = useAuth();

  const fetchProjects = useCallback(async (): Promise<
    ServiceResult<Project[]>
  > => {
    const response = await fetch("/api/projects", {
      headers: token ? { Authorization: `Bearer ${token}` } : undefined,
    });
    if (!response.ok) {
      const errorText = await response.text();
      return { success: false, error: errorText };
    }
    const projects = await response.json();
    return {
      success: true,
      data: projects.map((p: Project) => ({
        ...p,
        createdAt: new Date(p.createdAt),
        updatedAt: new Date(p.updatedAt),
      })),
    };
  }, [token]);

  const getProjectById = useCallback(
    async (id: string): Promise<ServiceResult<Project>> => {
      const response = await fetch(`/api/projects/${id}`, {
        headers: token ? { Authorization: `Bearer ${token}` } : undefined,
      });
      if (!response.ok) {
        const errorText = await response.text();
        return { success: false, error: errorText };
      }
      const p = await response.json();
      return {
        success: true,
        data: {
          ...p,
          createdAt: new Date(p.createdAt),
          updatedAt: new Date(p.updatedAt),
        },
      };
    },
    [token]
  );

  const createProject = useCallback(
    async (dto: ProjectDto): Promise<ServiceResult<Project>> => {
      const response = await fetch("/api/projects", {
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
      const p = await response.json();
      return {
        success: true,
        data: {
          ...p,
          createdAt: new Date(p.createdAt),
          updatedAt: new Date(p.updatedAt),
        },
      };
    },
    [token]
  );

  const updateProject = useCallback(
    async (id: string, dto: ProjectDto): Promise<ServiceResult<Project>> => {
      const response = await fetch(`/api/projects/${id}`, {
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
      const p = await response.json();
      return {
        success: true,
        data: {
          ...p,
          createdAt: new Date(p.createdAt),
          updatedAt: new Date(p.updatedAt),
        },
      };
    },
    [token]
  );

  const deleteProject = useCallback(
    async (id: string): Promise<ServiceResult<null>> => {
      const response = await fetch(`/api/projects/${id}`, {
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
    fetchProjects,
    getProjectById,
    createProject,
    updateProject,
    deleteProject,
  };
};
