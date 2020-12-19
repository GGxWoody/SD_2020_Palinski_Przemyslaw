import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { League } from '../_models/league';
import { LeaguesService } from '../_services/leagues.service';

@Injectable()
export class LeagueListResolver implements Resolve<League[]> {
    pageNumber = 1;
    pageSize = 6;

    constructor(private leagueService: LeaguesService, private router: Router,
                private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<League[]> {
        return this.leagueService.getLeagues(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving data');
                this.router.navigate(['/']);
                return of(null);
            })
        );
    }
}
