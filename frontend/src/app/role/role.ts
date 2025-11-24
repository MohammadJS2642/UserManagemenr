import { Component, inject, signal } from '@angular/core';
import { RolseService } from './infrastructure/role-service';
import { RoleModel } from './domain/Role.Model';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CreateRole } from './domain/create-role.model';

@Component({
  selector: 'app-role',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './role.html',
  styleUrl: './role.css',
})
export class Role {
  private readonly roleService = inject(RolseService);
  roles = signal<RoleModel[] | null>([]);
  requestForm = new FormGroup({
    name: new FormControl(''),
  });
  /**
   *
   */
  constructor() {
    this.getAll();
  }

  getAll() {
    this.roleService.getAll().subscribe({
      next: (res) => this.roles.set(res),
      error: (err) => console.log(err),
    });
  }

  createRole() {
    const request: CreateRole = this.requestForm.value as CreateRole;
    this.roleService.createUser(request).subscribe({
      next: (res) => {
        this.requestForm.reset();
        this.getAll();
      },
      error: (err) => console.log('create role error: ', err),
    });
  }
}
