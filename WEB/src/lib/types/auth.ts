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
export type ForgetPasswordDto = {
  email: string;
}

export type ResetPasswordDto =  {
  email: string;
  newPassword: string;
  code: string;
}

export type ConfirmEmailDto = {
  code: string;
}