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
