import { HttpClient } from '@angular/common/http';
import { inject, Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserModel } from '../domain/user.model';
import { CreateUser } from '../domain/create-user.mode';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly baseUrl = 'https://localhost:7091/api/users';
  private readonly http = inject(HttpClient);

  getAll(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(`${this.baseUrl}/GetAllUsers`);
  }

  createUser(request: CreateUser): Observable<UserModel> {
    return this.http.post<UserModel>(`${this.baseUrl}`, request);
  }

  delete(id: number) {
    return this.http.get(`${this.baseUrl}/disableuser?id=${id}`);
  }
}
