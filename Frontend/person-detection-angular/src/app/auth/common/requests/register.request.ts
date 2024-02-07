export class RegisterRequest {
  email: string = '';
  password: string = '';
  userName: string = '';

  constructor(email: string, password: string, userName: string) {
    this.email = email;
    this.password = password;
    this.userName = userName;
  }
}
