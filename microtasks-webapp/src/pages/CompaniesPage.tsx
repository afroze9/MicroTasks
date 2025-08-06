import { useEffect, useState } from "react";
import { useCompanyService } from "../services/companyService";
import type { Company } from "../services/companyService";
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
  Chip,
  Box,
  IconButton as MuiIconButton,
  Container,
  Stack,
  Typography,
  Button,
} from "@mui/material";

export default function CompaniesPage() {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [error, setError] = useState<string>("");
  const [dialogOpen, setDialogOpen] = useState(false);
  const [editingCompany, setEditingCompany] = useState<Company | null>(null);
  const [formName, setFormName] = useState("");
  const [formTags, setFormTags] = useState<string[]>([]);
  const [tagInput, setTagInput] = useState("");
  const { isAuthenticated } = useAuth();

  const { fetchCompanies, createCompany, updateCompany, deleteCompany } =
    useCompanyService();

  useEffect(() => {
    if (!isAuthenticated) return;
    fetchCompanies().then((result) => {
      if (result.success) {
        setCompanies(result.data);
        setError("");
      } else {
        setError(result.error);
      }
    });
  }, [isAuthenticated, fetchCompanies]);

  const handleOpenCreate = () => {
    setEditingCompany(null);
    setFormName("");
    setFormTags([]);
    setDialogOpen(true);
  };

  const handleOpenEdit = async (company: Company) => {
    setEditingCompany(company);
    setFormName(company.name);
    setFormTags(company.tags ? company.tags.map((t) => t.value) : []);
    setDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
    setEditingCompany(null);
    setFormName("");
    setFormTags([]);
    setTagInput("");
  };

  const handleSave = async () => {
    const dto = {
      name: formName,
      tags: formTags.map((value) => ({ value })),
    };
    let result;
    if (editingCompany) {
      result = await updateCompany(editingCompany.id, dto);
    } else {
      result = await createCompany(dto);
    }
    if (result.success) {
      const updated = await fetchCompanies();
      if (updated.success) {
        setCompanies(updated.data);
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
    if (!window.confirm("Are you sure you want to delete this company?"))
      return;
    const result = await deleteCompany(id);
    if (result.success) {
      const updated = await fetchCompanies();
      if (updated.success) {
        setCompanies(updated.data);
        setError("");
      } else {
        setError(updated.error);
      }
    } else {
      setError(result.error);
    }
  };

  const handleTagAdd = () => {
    if (tagInput.trim() && !formTags.includes(tagInput.trim())) {
      setFormTags([...formTags, tagInput.trim()]);
      setTagInput("");
    }
  };

  const handleTagDelete = (tag: string) => {
    setFormTags(formTags.filter((t) => t !== tag));
  };

  if (!isAuthenticated) {
    return <p>Please log in to view companies.</p>;
  }

  return (
    <Container sx={{ mb: 4 }}>
      <Stack
        direction="row"
        alignItems="center"
        justifyContent="space-between"
        sx={{ mb: 2 }}
      >
        <Typography variant="h4">Companies</Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleOpenCreate}
        >
          Create New Company
        </Button>
      </Stack>
      <Link to="/">Go to Home</Link>

      {error && <Typography color="error">{error}</Typography>}

      <Box sx={{ height: 600, mt: 2 }}>
        <DataGrid<Company>
          rows={companies}
          columns={[
            { field: "name", headerName: "Name", flex: 1 },
            {
              field: "createdAt",
              headerName: "Created At",
              flex: 1,
              renderCell: (params: GridRenderCellParams<Company>) => {
                const date = params.row.createdAt;
                return date ? new Date(date).toLocaleString() : "-";
              },
            },
            {
              field: "updatedAt",
              headerName: "Updated At",
              flex: 1,
              renderCell: (params: GridRenderCellParams<Company>) => {
                const date = params.row.updatedAt;
                return date ? new Date(date).toLocaleString() : "-";
              },
            },
            {
              field: "tags",
              headerName: "Tags",
              flex: 1,
              renderCell: (params: GridRenderCellParams<Company>) =>
                Array.isArray(params.value) && params.value.length > 0
                  ? (params.value as Company["tags"]).map((tag) => (
                      <Chip
                        key={tag.value}
                        label={tag.value}
                        size="small"
                        sx={{ mr: 0.5 }}
                      />
                    ))
                  : "-",
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
                  <MuiIconButton
                    color="primary"
                    size="small"
                    aria-label="edit"
                    onClick={() => handleOpenEdit(params.row)}
                  >
                    <EditIcon />
                  </MuiIconButton>
                  <MuiIconButton
                    color="error"
                    size="small"
                    aria-label="delete"
                    sx={{ ml: 1 }}
                    onClick={() => handleDelete(params.row.id)}
                  >
                    <DeleteIcon />
                  </MuiIconButton>
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
          {editingCompany ? "Edit Company" : "Create Company"}
        </DialogTitle>
        <DialogContent>
          <TextField
            label="Name"
            value={formName}
            onChange={(e) => setFormName(e.target.value)}
            fullWidth
            sx={{ mb: 2 }}
          />
          <Box sx={{ display: "flex", alignItems: "center", mb: 2 }}>
            <TextField
              label="Add Tag"
              value={tagInput}
              onChange={(e) => setTagInput(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  e.preventDefault();
                  handleTagAdd();
                }
              }}
              sx={{ mr: 2 }}
            />
            <Button variant="outlined" onClick={handleTagAdd} sx={{ mr: 2 }}>
              Add
            </Button>
            {formTags.map((tag) => (
              <Chip
                key={tag}
                label={tag}
                onDelete={() => handleTagDelete(tag)}
                sx={{ mr: 1 }}
              />
            ))}
          </Box>
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
            {editingCompany ? "Update" : "Create"}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}
