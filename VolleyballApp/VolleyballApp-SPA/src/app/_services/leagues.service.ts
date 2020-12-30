import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { League } from '../_models/league';
import { Match } from '../_models/match';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root',
})
export class LeaguesService {
  baseUrl = environment.apiUrl;


  constructor(private http: HttpClient) {}

  getLeague(id): Observable<League> {
    return this.http.get<League>(this.baseUrl + 'league/' + id);
  }

  getLeagues(page?, itemsPerPage?): Observable<PaginatedResult<League[]>> {
    const paginatedResult: PaginatedResult<League[]> = new PaginatedResult<
      League[]
    >();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<League[]>(this.baseUrl + 'league', { observe: 'response', params })
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

  getLeagueMatches(id, page?, itemsPerPage?): Observable<PaginatedResult<Match[]>> {
    const paginatedResult: PaginatedResult<Match[]> = new PaginatedResult<
    Match[]
    >();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<Match[]>(this.baseUrl + 'league/' + id + '/matches', { observe: 'response', params })
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

  joinLeague(leagueId, userId) {
    return this.http.put(this.baseUrl + 'league/' + userId + '/' + leagueId, {});
  }

  startLeague(leagueId) {
    return this.http.put(this.baseUrl + 'league/' + leagueId, {});
  }

  createLeague(model: any) {
    return this.http.post(this.baseUrl + 'league', model);
  }
}
