import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { TeamService } from '../_services/team.service';
import { Team } from '../_models/team';

@Injectable()
export class TeamDetailResolver implements Resolve<Team> {
    constructor(private teamService: TeamService, private router: Router, private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Team> {
        return this.teamService.getTeam(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving data');
                this.router.navigate(['/teams']);
                return of(null);
            })
        );
    }
}
