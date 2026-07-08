export type Result<T> = {
  isSuccess: boolean;
  value: T | null;
  error: string | null;
};
