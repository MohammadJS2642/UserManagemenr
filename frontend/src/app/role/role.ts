import { Component, inject, signal } from '@angular/core';
import { RolseService } from './infrastructure/role-service';
import { RoleModel } from './domain/Role.Model';

@Component({
  selector: 'app-role',
  imports: [],
  templateUrl: './role.html',
  styleUrl: './role.css',
})
export class Role {
  private readonly roleService = inject(RolseService);
  roles = signal<RoleModel[] | null>([]);

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
}
