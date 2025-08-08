import { useEffect, useState } from "react";
import { useProjectService } from "../services/projectService";
import { useAuth } from "../auth/useAuth";
import { Link } from "react-router-dom";
import { DataGrid, type GridRenderCellParams } from "@mui/x-data-grid";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Box,
  IconButton as MuiIconButton,
  Container,
  Stack,
  Typography,
  Button,
  MenuItem,
  Select,
  InputLabel,
  FormControl,
} from "@mui/material";
import type { Project, ProjectStatus } from "../types/projectTypes";

const PROJECT_STATUS_OPTIONS: ProjectStatus[] = [
  "New",
  "Active",
  "Completed",
  "Archived",
  "Cancelled",
];

export default function ProjectsPage() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [error, setError] = useState<string>("");
  const [dialogOpen, setDialogOpen] = useState(false);
  const [editingProject, setEditingProject] = useState<Project | null>(null);
  const [formName, setFormName] = useState("");
  const [formDescription, setFormDescription] = useState("");
  const [formStatus, setFormStatus] = useState<ProjectStatus>("New");
  const { isAuthenticated, hasRole, isInRoles } = useAuth();

  const { fetchProjects, createProject, updateProject, deleteProject } =
    useProjectService();

  useEffect(() => {
    if (!isAuthenticated) return;
    fetchProjects().then((result) => {
      if (result.success) {
        setProjects(result.data);
        setError("");
      } else {
        setError(result.error);
      }
    });
  }, [isAuthenticated, fetchProjects]);

  const handleOpenCreate = () => {
    setEditingProject(null);
    setFormName("");
    setFormDescription("");
    setFormStatus("New");
    setDialogOpen(true);
  };

  const handleOpenEdit = async (project: Project) => {
    setEditingProject(project);
    setFormName(project.name);
    setFormDescription(project.description || "");
    setFormStatus(project.status || "New");
    setDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
    setEditingProject(null);
    setFormName("");
    setFormDescription("");
    setFormStatus("New");
  };

  const handleSave = async () => {
    const dto = {
      name: formName,
      description: formDescription,
      status: formStatus,
    };
    let result;
    if (editingProject) {
      result = await updateProject(editingProject.id, dto);
    } else {
      result = await createProject(dto);
    }
    if (result.success) {
      const updated = await fetchProjects();
      if (updated.success) {
        setProjects(updated.data);
        setError("");
      } else {
        setError(updated.error);
      }
      handleCloseDialog();
    } else {
      setError(result.error);
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm("Are you sure you want to delete this project?"))
      return;
    const result = await deleteProject(id);
    if (result.success) {
      const updated = await fetchProjects();
      if (updated.success) {
        setProjects(updated.data);
        setError("");
      } else {
        setError(updated.error);
      }
    } else {
      setError(result.error);
    }
  };

  if (!isAuthenticated) {
    return <p>Please log in to view projects.</p>;
  }

  return (
    <Container sx={{ mb: 4 }}>
      <Stack
        direction="row"
        alignItems="center"
        justifyContent="space-between"
        sx={{ mb: 2 }}
      >
        <Typography variant="h4">Projects</Typography>
        {isInRoles("project-api", [
          "project_manager",
          "project_contributor",
        ]) && (
          <Button
            variant="contained"
            color="primary"
            startIcon={<AddIcon />}
            onClick={handleOpenCreate}
          >
            Create New Project
          </Button>
        )}
      </Stack>
      <Link to="/">Go to Home</Link>

      {error && <Typography color="error">{error}</Typography>}

      <Box sx={{ height: 600, mt: 2 }}>
        <DataGrid<Project>
          rows={projects}
          columns={[
            { field: "name", headerName: "Name", flex: 1 },
            {
              field: "description",
              headerName: "Description",
              flex: 1,
            },
            {
              field: "status",
              headerName: "Status",
              flex: 1,
            },
            {
              field: "createdAt",
              headerName: "Created At",
              flex: 1,
              renderCell: (params: GridRenderCellParams<Project>) => {
                const date = params.row.createdAt;
                return date ? new Date(date).toLocaleString() : "-";
              },
            },
            {
              field: "updatedAt",
              headerName: "Updated At",
              flex: 1,
              renderCell: (params: GridRenderCellParams<Project>) => {
                const date = params.row.updatedAt;
                return date ? new Date(date).toLocaleString() : "-";
              },
            },
            {
              field: "actions",
              headerName: "Actions",
              flex: 1,
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
          <FormControl fullWidth sx={{ mb: 2 }}>
            <InputLabel>Status</InputLabel>
            <Select
              label="Status"
              value={formStatus}
              onChange={(e) => setFormStatus(e.target.value as ProjectStatus)}
            >
              {PROJECT_STATUS_OPTIONS.map((status) => (
                <MenuItem key={status} value={status}>
                  {status}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
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
