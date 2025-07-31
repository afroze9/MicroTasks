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

export async function fetchCompanies(): Promise<Company[]> {
  const response = await fetch("/companies");
  const companies = await response.json();

  return companies.map((c: Company) => ({
    ...c,
    createdAt: new Date(c.createdAt),
    updatedAt: new Date(c.updatedAt),
  }));
}
