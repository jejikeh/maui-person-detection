import { Injectable, signal } from '@angular/core';
import { UserInterface } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  currentUser = signal<UserInterface | undefined | null>(null);
}
