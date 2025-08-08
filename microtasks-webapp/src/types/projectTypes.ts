export type ProjectStatus = "NotStarted" | "Active" | "Completed" | "OnHold";

export type Project = {
  id: string;
  name: string;
  description: string;
  status: ProjectStatus;
  createdAt: string;
  updatedAt: string;
};

export type ProjectDto = {
  name: string;
  description: string;
  status: ProjectStatus;
};

export type WorkItemStatus = "NotStarted" | "Active" | "Completed" | "Blocked";

export type WorkItem = {
  id: string;
  projectId: string;
  title: string;
  description: string;
  status: WorkItemStatus;
  createdAt: string;
  updatedAt: string;
};

export type WorkItemDto = {
  title: string;
  description: string;
  status: WorkItemStatus;
};
