import type { WorkItem, WorkItemDto } from "../types/projectTypes";
import type { ServiceResult } from "../types/ServiceResult";

export async function fetchAllWorkItems(): Promise<ServiceResult<WorkItem[]>> {
  const res = await fetch(`/api/workitems`);
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const workItems = await res.json();
  return { success: true, data: workItems };
}

export async function fetchWorkItemById(
  id: string
): Promise<ServiceResult<WorkItem>> {
  const res = await fetch(`/api/workitems/${id}`);
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const workItem = await res.json();
  return { success: true, data: workItem };
}

export async function getProjectWorkItems(
  projectId: string
): Promise<ServiceResult<WorkItem[]>> {
  const res = await fetch(`/api/projects/${projectId}/workitems`);
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const workItems = await res.json();
  return { success: true, data: workItems };
}

export async function createWorkItem(
  payload: WorkItemDto
): Promise<ServiceResult<WorkItem>> {
  const res = await fetch(`/api/workitems`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const workItem = await res.json();
  return { success: true, data: workItem };
}

export async function updateWorkItem(
  id: string,
  payload: WorkItemDto
): Promise<ServiceResult<WorkItem>> {
  const res = await fetch(`/api/workitems/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  const workItem = await res.json();
  return { success: true, data: workItem };
}

export async function deleteWorkItem(id: string): Promise<ServiceResult<null>> {
  const res = await fetch(`/api/workitems/${id}`, {
    method: "DELETE",
  });
  if (!res.ok) {
    const errorText = await res.text();
    return { success: false, error: errorText };
  }
  return { success: true, data: null };
}
