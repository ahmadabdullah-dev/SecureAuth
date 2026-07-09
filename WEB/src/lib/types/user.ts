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
  roles: string[];
}