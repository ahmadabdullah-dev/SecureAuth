export type UserDto = {
  userName: string;
  firstName: string | null;
  lastName: string | null;
  email: string;
  phoneNumber: string | null;
  country: string | null;
  emailConfirmed: boolean;
  birthDate: string | null; 
  joinedDate: string;
  role: string;
}
export type RequestUpdateEmailDto = {
  newEmail: string;
}
export type UpdateEmailDto = {
  code: string  
}
export type UpdateCurrentUserDto = {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  country: string;
  dateOfBirth: string;
}
export type updateUserNameDto = {
  newUserName: string
}