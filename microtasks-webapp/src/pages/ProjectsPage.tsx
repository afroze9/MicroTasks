import { useEffect, useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Box,
  Container,
  Button,
  Chip,
} from "@mui/material";
import {
  DataGrid as MuiDataGrid,
  type GridRenderCellParams,
} from "@mui/x-data-grid";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import CloseIcon from "@mui/icons-material/Close";
import { IconButton as MuiIconButton } from "@mui/material";
import { useAuth } from "../auth/useAuth";

interface Project {
  id: string;
  name: string;
  description: string;
  status: string;
  createdAt: string;
  updatedAt: string;
}

export default function ProjectsPage() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [editingProject, setEditingProject] = useState<Project | null>(null);
  const [formName, setFormName] = useState("");
  const [formDescription, setFormDescription] = useState("");
  const [formStatus, setFormStatus] = useState("");
  const { hasRole, isInRoles } = useAuth();

  useEffect(() => {
    fetch("/projects")
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch projects");
        return res.json();
      })
      .then(setProjects)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  const handleOpenCreate = () => {
    setEditingProject(null);
    setFormName("");
    setFormDescription("");
    setFormStatus("");
    setDialogOpen(true);
  };

  const handleOpenEdit = (project: Project) => {
    setEditingProject(project);
    setFormName(project.name);
    setFormDescription(project.description);
    setFormStatus(project.status);
    setDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
  };

  const handleSave = async () => {
    const payload = {
      name: formName,
      description: formDescription,
      status: formStatus,
    };
    try {
      if (editingProject) {
        // Update
        const res = await fetch(`/projects/${editingProject.id}`, {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(payload),
        });
        if (!res.ok) throw new Error("Failed to update project");
      } else {
        // Create
        const res = await fetch(`/projects`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(payload),
        });
        if (!res.ok) throw new Error("Failed to create project");
      }
      // Refresh
      const res = await fetch("/projects");
      setProjects(await res.json());
      handleCloseDialog();
    } catch (err: any) {
      setError(err.message);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      const res = await fetch(`/projects/${id}`, { method: "DELETE" });
      if (!res.ok) throw new Error("Failed to delete project");
      setProjects(projects.filter((p) => p.id !== id));
    } catch (err: any) {
      setError(err.message);
    }
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Box sx={{ display: "flex", alignItems: "center", mb: 2 }}>
        <Box sx={{ flexGrow: 1 }}>
          <h1>Projects</h1>
        </Box>
        {isInRoles("project-api", [
          "project_manager",
          "project_contributor",
        ]) && (
          <Button
            variant="contained"
            color="primary"
            onClick={handleOpenCreate}
          >
            Create Project
          </Button>
        )}
      </Box>
      {error && <Box sx={{ color: "error.main", mb: 2 }}>{error}</Box>}
      <Box sx={{ height: 500, width: "100%" }}>
        <MuiDataGrid
          rows={projects}
          columns={[
            { field: "name", headerName: "Name", flex: 1 },
            { field: "description", headerName: "Description", flex: 2 },
            { field: "status", headerName: "Status", flex: 1 },
            {
              field: "createdAt",
              headerName: "Created At",
              flex: 1,
              valueFormatter: (params) =>
                new Date(params.value).toLocaleString(),
            },
            {
              field: "updatedAt",
              headerName: "Updated At",
              flex: 1,
              valueFormatter: (params) =>
                new Date(params.value).toLocaleString(),
            },
            {
              field: "actions",
              headerName: "Actions",
              sortable: false,
              filterable: false,
              align: "right",
              renderCell: (params: GridRenderCellParams) => (
                <>
                  {isInRoles("project-api", [
                    "project_manager",
                    "project_contributor",
                  ]) && (
                    <MuiIconButton
                      color="primary"
                      size="small"
                      aria-label="edit"
                      onClick={() => handleOpenEdit(params.row)}
                    >
                      <EditIcon />
                    </MuiIconButton>
                  )}
                  {hasRole("project-api", "project_manager") && (
                    <MuiIconButton
                      color="error"
                      size="small"
                      aria-label="delete"
                      sx={{ ml: 1 }}
                      onClick={() => handleDelete(params.row.id)}
                    >
                      <DeleteIcon />
                    </MuiIconButton>
                  )}
                </>
              ),
            },
          ]}
          getRowId={(row) => row.id}
          disableRowSelectionOnClick
          pageSizeOptions={[10, 25, 50]}
        />
      </Box>

      <Dialog
        open={dialogOpen}
        onClose={handleCloseDialog}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>
          {editingProject ? "Edit Project" : "Create Project"}
        </DialogTitle>
        <DialogContent>
          <TextField
            label="Name"
            value={formName}
            onChange={(e) => setFormName(e.target.value)}
            fullWidth
            sx={{ mb: 2 }}
          />
          <TextField
            label="Description"
            value={formDescription}
            onChange={(e) => setFormDescription(e.target.value)}
            fullWidth
            sx={{ mb: 2 }}
          />
          <TextField
            label="Status"
            value={formStatus}
            onChange={(e) => setFormStatus(e.target.value)}
            fullWidth
            sx={{ mb: 2 }}
          />
        </DialogContent>
        <DialogActions>
          <Button
            onClick={handleCloseDialog}
            color="secondary"
            startIcon={<CloseIcon />}
          >
            Cancel
          </Button>
          <Button onClick={handleSave} variant="contained" color="primary">
            {editingProject ? "Update" : "Create"}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}
