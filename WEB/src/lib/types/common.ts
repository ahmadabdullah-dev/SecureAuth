export type Result<T> = {
  isSuccess: boolean;
  value: T | null;
  error: string | null;
};
export type PaginationParams = {
  page: number;
  pageSize: number;
}
export type PaginatedList<T> = {
  items: T[];
  currentPage: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
};