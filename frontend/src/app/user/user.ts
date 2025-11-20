import { Component, inject } from '@angular/core';
import { UserService } from './infrastructure/user-service';
import { UserModel } from './domain/user.model';

@Component({
  selector: 'app-user',
  imports: [],
  templateUrl: './user.html',
  styleUrl: './user.css',
})
export class User {
  private readonly userService = inject(UserService);
  users: UserModel[] | null = [];

  loadUsers() {
    this.userService.getAll().subscribe((res) => (this.users = res));
  }

  remove(id: number) {
    this.userService.delete(id);
  }
}
