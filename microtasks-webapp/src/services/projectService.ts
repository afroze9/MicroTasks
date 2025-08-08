import type { Project } from "../types/projectTypes";
import type { ServiceResult } from "../types/ServiceResult";

export async function getProjects(): Promise<ServiceResult<Project[]>> {
  const res = await fetch("/projects");
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const projects = await res.json();
  return { success: true, data: projects };
}

export async function createProject(
  payload: Partial<Project>
): Promise<ServiceResult<Project>> {
  const res = await fetch("/projects", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const project = await res.json();
  return { success: true, data: project };
}

export async function updateProject(
  id: string,
  payload: Partial<Project>
): Promise<ServiceResult<Project>> {
  const res = await fetch(`/projects/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const project = await res.json();
  return { success: true, data: project };
}

export async function deleteProject(id: string): Promise<ServiceResult<null>> {
  const res = await fetch(`/projects/${id}`, { method: "DELETE" });
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  return { success: true, data: null };
}
