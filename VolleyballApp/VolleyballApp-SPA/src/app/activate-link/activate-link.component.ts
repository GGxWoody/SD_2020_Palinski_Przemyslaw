import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-activate-link',
  templateUrl: './activate-link.component.html',
  styleUrls: ['./activate-link.component.scss']
})
export class ActivateLinkComponent implements OnInit {

  constructor(private authService: AuthService, private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.authService.activate(+this.route.snapshot.paramMap.get('id')).subscribe(next => {
      this.alertify.success('Activation successfull');
    }, error => {
      this.alertify.error(error);
    });
  }

}
