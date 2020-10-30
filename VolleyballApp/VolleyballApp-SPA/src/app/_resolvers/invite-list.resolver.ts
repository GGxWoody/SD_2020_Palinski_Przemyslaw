import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FriendInviteService } from '../_services/friend-invite.service';
import { Invite } from '../_models/invite';

@Injectable()
export class InviteListResolver implements Resolve<Invite[]> {
    pageNumber = 1;
    pageSize = 4;

    constructor(private inviteService: FriendInviteService, private router: Router,
                private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Invite[]> {
        return this.inviteService.getInvites(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving data');
                this.router.navigate(['/']);
                return of(null);
            })
        );
    }
}
