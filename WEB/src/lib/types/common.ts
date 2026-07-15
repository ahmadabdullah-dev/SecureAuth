export type Result<T> = {
  isSuccess: boolean;
  value: T | null;
  error: string | null;
};
export type PaginationParams = {
  page: number;
  pageSize: number;
}