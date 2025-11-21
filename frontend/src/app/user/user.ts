import { Component, effect, inject, signal } from '@angular/core';
import { UserService } from './infrastructure/user-service';
import { UserModel } from './domain/user.model';
import { CreateUser } from './domain/create-user.mode';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './user.html',
  styleUrl: './user.css',
})
export class User {
  private readonly userService = inject(UserService);
  users = signal<UserModel[] | null>([]);
  requestForm = new FormGroup({
    username: new FormControl(''),
    email: new FormControl(''),
    passwordHash: new FormControl(''),
  });

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

  createUser() {
    console.log(this.requestForm.value as CreateUser);
    const request: CreateUser = this.requestForm.value as CreateUser;
    this.userService.createUser(request).subscribe({
      next: (res) => {
        console.log('user created: ', res);
        this.requestForm.reset();
        this.loadUsers();
      },
      error: (err) => console.log('error', err),
    });
  }

  disableUser(id: number) {
    this.userService.delete(id).subscribe({
      next: (res) => {
        console.log('User removed: ', res);
        this.loadUsers();
      },
      error: (err) => console.log('erro: ', err),
    });
  }
}
