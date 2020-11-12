import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { TeamService } from '../_services/team.service';
import { Team } from '../_models/team';

@Injectable()
export class TeamListResolver implements Resolve<Team[]> {
    pageNumber = 1;
    pageSize = 6;

    constructor(private teamService: TeamService, private router: Router,
                private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Team[]> {
        return this.teamService.getTeams(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving data');
                this.router.navigate(['/']);
                return of(null);
            })
        );
    }
}
