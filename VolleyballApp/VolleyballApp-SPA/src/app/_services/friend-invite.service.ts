import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Invite } from '../_models/invite';
import { PaginatedResult } from '../_models/pagination';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class FriendInviteService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private authService: AuthService) {}

  sendInvite(userId: number, id: number) {
    return this.http.post(this.baseUrl + 'invites/' + userId + '/friend/' + id, {});
  }

  getInvites(page?, itemsPerPage?): Observable<PaginatedResult<Invite[]>> {
    const paginatedResult: PaginatedResult<Invite[]> = new PaginatedResult<
      Invite[]
    >();
    const userId = this.authService.decodedToken.nameid;
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<Invite[]>(this.baseUrl + 'invites/' + userId, { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  acceptInvite(userId: number, id: number) {
    return this.http.put(this.baseUrl + 'invites/' + userId + '/friend/' + id, {});
  }
}
