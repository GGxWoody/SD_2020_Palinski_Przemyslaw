import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Match } from '../_models/match';
import { PaginatedResult } from '../_models/pagination';
import { Score } from '../_models/score';

@Injectable({
  providedIn: 'root',
})
export class MatchService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMatch(id): Observable<Match> {
    return this.http.get<Match>(this.baseUrl + 'match/' + id);
  }

  getMatches(page?, itemsPerPage?): Observable<PaginatedResult<Match[]>> {
    const paginatedResult: PaginatedResult<Match[]> = new PaginatedResult<
      Match[]
    >();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<Match[]>(this.baseUrl + 'match', { observe: 'response', params })
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

  setLocationAndTime(id: number, location: Location) {
    return this.http.put(this.baseUrl + 'match/' + id + '/location', location);
  }

  setScore(id: number, score: Score) {
    return this.http.put(this.baseUrl + 'match/' + id + '/score', score);
  }
}
