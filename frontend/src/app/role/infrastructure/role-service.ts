import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RoleModel } from '../domain/Role.Model';

@Injectable({ providedIn: 'root' })
export class RolseService {
  private readonly baseUrl = 'https://localhost:7091/api/role';
  private readonly http = inject(HttpClient);

  getAll(): Observable<RoleModel[]> {
    return this.http.get<RoleModel[]>(`${this.baseUrl}/GetRoles`);
  }
}
