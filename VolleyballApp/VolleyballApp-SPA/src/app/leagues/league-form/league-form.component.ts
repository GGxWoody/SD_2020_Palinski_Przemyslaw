import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { LeaguesService } from 'src/app/_services/leagues.service';

@Component({
  selector: 'app-league-form',
  templateUrl: './league-form.component.html',
  styleUrls: ['./league-form.component.scss']
})
export class LeagueFormComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  leagueForm: FormGroup;
  league: any;
  user: User = JSON.parse(localStorage.getItem('user'));

  constructor(private fb: FormBuilder, private leagueService: LeaguesService,
              private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.createLeagueForm();
  }

  createLeagueForm() {
    this.leagueForm = this.fb.group({
      country: ['', Validators.required],
      city: ['', Validators.required],
      description: ['', Validators.required],
      teamLimit: ['', [Validators.required, Validators.pattern('^[0-8]*$'), Validators.maxLength(1)]],
      closedSignUp: [null, Validators.required],
      creatorId: [this.user.id, Validators.required]
    });
  }

  createLeague() {
    if (this.leagueForm.valid) {
      this.league = Object.assign({}, this.leagueForm.value);
      this.leagueService.createLeague(this.league).subscribe(() => {
        this.alertify.success('League created successfully');
        this.router.navigate(['/leagues']);
      } , error => {
          this.alertify.error(error);
      });
   }
  }

  cancel() {
    this.cancelRegister.emit(false);
    this.alertify.message('Cancelled');
  }
}
