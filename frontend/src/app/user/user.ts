import { Component, effect, inject, signal } from '@angular/core';
import { UserService } from './infrastructure/user-service';
import { UserModel } from './domain/user.model';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [],
  templateUrl: './user.html',
  styleUrl: './user.css',
})
export class User {
  private readonly userService = inject(UserService);
  users = signal<UserModel[] | null>([]);

  /**
   *
   */
  constructor() {
    effect(() => {
      this.userService.getAll().subscribe({
        next: (res) => this.users.set(res),
        error: (err) => console.error(err),
      });
    });
  }

  loadUsers() {
    this.userService.getAll().subscribe((res) => this.users.set(res));
  }

  remove(id: number) {
    this.userService.delete(id);
  }
}
