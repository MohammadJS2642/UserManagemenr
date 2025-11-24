import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RoleModel } from '../domain/Role.Model';
import { CreateRole } from '../domain/create-role.model';

@Injectable({ providedIn: 'root' })
export class RolseService {
  private readonly baseUrl = 'https://localhost:7091/api/role';
  private readonly http = inject(HttpClient);

  getAll(): Observable<RoleModel[]> {
    return this.http.get<RoleModel[]>(`${this.baseUrl}/GetRoles`);
  }

  createUser(request: CreateRole): Observable<RoleModel> {
    return this.http.post<CreateRole>(`${this.baseUrl}`, request);
  }
}
