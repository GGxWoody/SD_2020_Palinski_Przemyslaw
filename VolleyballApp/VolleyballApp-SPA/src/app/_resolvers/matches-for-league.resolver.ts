import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Match } from '../_models/match';
import { LeaguesService } from '../_services/leagues.service';

@Injectable()
export class MatchesForLeagueResolver implements Resolve<Match[]> {
    pageNumber = 1;
    pageSize = 10;

    constructor(private leagueService: LeaguesService, private router: Router,
                private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Match[]> {
        return this.leagueService.getLeagueMatches(route.params.id, this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving data');
                this.router.navigate(['/legues']);
                return of(null);
            })
        );
    }
}
