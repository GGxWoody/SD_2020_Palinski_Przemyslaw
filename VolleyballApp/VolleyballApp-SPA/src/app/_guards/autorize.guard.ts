import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AutorizeGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router,
              private alertify: AlertifyService) { }
  canActivate(): boolean {
    if (this.authService.loggedIn() && !this.authService.activatedMail()) {
      return true;
    }

    this.alertify.error('You shall not pass!');
    this.router.navigate(['/home']);
    return false;
  }

}
