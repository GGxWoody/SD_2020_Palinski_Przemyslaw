import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Match } from '../_models/match';
import { MatchService } from '../_services/match.service';


@Injectable()
export class MatchDetailResolver implements Resolve<Match> {
    constructor(private matchService: MatchService, private router: Router, private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Match> {
        return this.matchService.getMatch(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving data');
                this.router.navigate(['/matches']);
                return of(null);
            })
        );
    }
}
