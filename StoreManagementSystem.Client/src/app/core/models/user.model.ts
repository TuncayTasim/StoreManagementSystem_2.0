// --- DTOs used for authentication requests/responses ---

export interface UserRegisterDTO {
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  password: string;
  roleId: number;
}

export interface UserLoginDTO {
  userName: string;
  password: string;
}

export interface UserResponseDTO {
  userId: number;
  userName: string;
  token: string;
  roleId: number;
}

export interface ForgotPasswordDTO {
  email: string;
}

export interface ResetPasswordDTO {
  token: string;
  newPassword: string;
  confirmPassword: string;
}
