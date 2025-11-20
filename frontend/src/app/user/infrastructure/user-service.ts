import { HttpClient } from '@angular/common/http';
import { inject, Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserModel } from '../domain/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly baseUrl = 'https://localhost:7091/api/users';
  private readonly http = inject(HttpClient);

  getAll(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(`${this.baseUrl}/GetAllUsers`);
  }

  delete(id: number): Observable<void> {
    return this.http.get<void>(`${this.baseUrl}/disableuser/${id}`);
  }
}
