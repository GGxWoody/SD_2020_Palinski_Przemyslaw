import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { League } from '../_models/league';
import { LeaguesService } from '../_services/leagues.service';

@Injectable()
export class LeagueDetailResolver implements Resolve<League> {
    constructor(private leagueService: LeaguesService, private router: Router,
                private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<League> {
        return this.leagueService.getLeague(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving data');
                this.router.navigate(['/matches']);
                return of(null);
            })
        );
    }
}
