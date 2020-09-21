import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { Team } from '../_models/team';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getTeams(page?, itemsPerPage?): Observable<PaginatedResult<Team[]>> {
  const paginatedResult: PaginatedResult<Team[]> = new PaginatedResult<
  Team[]
  >();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }

  return this.http
  .get<Team[]>(this.baseUrl + 'teams', { observe: 'response', params })
  .pipe(
    map(response => {
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

getTeam(id): Observable<Team> {
  return this.http.get<Team>(this.baseUrl + 'teams/' + id);
}
}
