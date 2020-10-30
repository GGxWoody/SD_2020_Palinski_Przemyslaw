import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Team } from 'src/app/_models/team';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { TeamService } from 'src/app/_services/team.service';

@Component({
  selector: 'app-team-create',
  templateUrl: './team-create.component.html',
  styleUrls: ['./team-create.component.scss']
})
export class TeamCreateComponent implements OnInit {
  @Output() cancelCreation = new EventEmitter();
  team: Team;
  creationForm: FormGroup;
  constructor(private alertify: AlertifyService, private fb: FormBuilder, private TeamServ: TeamService, private router: Router) { }

  ngOnInit() {
    this.createCreationForm();
  }

  createCreationForm() {
    this.creationForm = this.fb.group({
      teamName: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  create() {
    if (this.creationForm.valid) {
      this.team = Object.assign({}, this.creationForm.value);
      this.TeamServ.createTeam(this.team).subscribe(() => {
        this.alertify.success('Registration succesfull');
      } , error => {
          this.alertify.error(error);
      }, () => {
          this.router.navigate(['/teams']);
      });
    }
  }


  cancel() {
    this.cancelCreation.emit(false);
    this.alertify.message('Cancelled');
  }

}
