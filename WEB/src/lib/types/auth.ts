export type RegisterUserDto = {
  userName: string;
  email: string;
  password: string;
};
export type LoginUserDto = {
  email: string,
  password: string,
  isPersistence: boolean
}