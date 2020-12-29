import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-activate',
  templateUrl: './activate.component.html',
  styleUrls: ['./activate.component.scss']
})
export class ActivateComponent implements OnInit {
  user: User = JSON.parse(localStorage.getItem('user'));

  constructor(private authService: AuthService, private alertify: AlertifyService) { }
  ngOnInit() {
  }

  resendMail() {
    this.authService.resendMail(this.user.id).subscribe(next => {
      this.alertify.success('Activation mail send successfully');
    }, error => {
      this.alertify.error(error);
    });
  }
}
