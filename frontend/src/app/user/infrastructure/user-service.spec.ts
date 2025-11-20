import { TestBed } from '@angular/core/testing';

import { UserService } from './user-service';
import { HttpTestingController } from '@angular/common/http/testing';
import { UserModel } from '../domain/user.model';

describe('UserService', () => {
  let service: UserService;
  let httpMockControll: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpTestingController],
      providers: [UserService],
    });
    service = TestBed.inject(UserService);
    httpMockControll = TestBed.inject(HttpTestingController);
  });
  afterEach(() => {
    httpMockControll.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('shout be return users from api (getAllUsers)', () => {
    const mockUsers: UserModel[] = [
      { id: 1, userName: 'mohammad', email: 'test@gmail.com' },
      { id: 2, userName: 'mohammad2', email: 'test2@gmail.com' },
    ];
    service.getAll().subscribe((users) => {
      expect(users.length).toBe(2);
      expect(users).toEqual(mockUsers);
    });

    const req = httpMockControll.expectOne('https://localhost:7091/api/users/getAllUsers');

    expect(req.request.method).toBe('GET');

    req.flush(mockUsers);
  });
});
