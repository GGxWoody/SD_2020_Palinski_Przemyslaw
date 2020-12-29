import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Match } from 'src/app/_models/match';
import { Pagination } from 'src/app/_models/pagination';
import { Score } from 'src/app/_models/score';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FriendInviteService } from 'src/app/_services/friend-invite.service';
import { MatchService } from 'src/app/_services/match.service';

@Component({
  selector: 'app-match-detail',
  templateUrl: './match-detail.component.html',
  styleUrls: ['./match-detail.component.scss']
})
export class MatchDetailComponent implements OnInit {
  match: Match;
  location: Location;
  score: Score;
  referees: User[];
  pagination: Pagination;
  user: User = JSON.parse(localStorage.getItem('user'));
  locationForm: FormGroup;
  scoreForm: FormGroup;

  constructor(private matchService: MatchService, private alertify: AlertifyService, private inviteService: FriendInviteService,
              private root: ActivatedRoute, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.match = data.match;
      this.referees = data.referees.result;
      this.pagination = data.referees.pagination;
    });
    this.createLocationForm();
    this.createScoreForm();
  }

  createLocationForm() {
    if (this.match.location.country === null) {
      this.locationForm = this.fb.group({
        city: [this.user.city, Validators.required],
        country: [this.user.country, Validators.required],
        adress: ['', Validators.required],
        timeOfMatch: [null, Validators.required]
      });
    } else {
      this.locationForm = this.fb.group({
        city: [this.match.location.city, Validators.required],
        country: [this.match.location.country, Validators.required],
        adress: [this.match.location.adress, Validators.required],
        timeOfMatch: [this.match.location.timeOfMatch, Validators.required]
      });
    }
  }

  createScoreForm() {
    if (this.match.score.firstTeamSets + this.match.score.secondTeamSets === 0) {
      this.scoreForm = this.fb.group({
        firstTeamSets: [null, Validators.required],
        secondTeamSets: [null, Validators.required],
        oneFirstTeam: [null, Validators.required],
        oneSecondTeam: [null, Validators.required],
        twoFirstTeam: [null, Validators.required],
        twoSecondTeam: [null, Validators.required],
        threeFirstTeam: [null, Validators.required],
        threeSecondTeam: [null, Validators.required],
        fourFirstTeam: [null, Validators.required],
        fourSecondTeam: [null, Validators.required],
        fiveFirstTeam: [null, Validators.required],
        fiveSecondTeam: [null, Validators.required]
      });
    } else {
      this.scoreForm = this.fb.group({
        firstTeamSets: [this.match.score.firstTeamSets, Validators.required],
        secondTeamSets: [this.match.score.secondTeamSets, Validators.required],
        oneFirstTeam: [this.match.score.oneFirstTeam, Validators.required],
        oneSecondTeam: [this.match.score.oneSecondTeam, Validators.required],
        twoFirstTeam: [this.match.score.twoFirstTeam, Validators.required],
        twoSecondTeam: [this.match.score.twoSecondTeam, Validators.required],
        threeFirstTeam: [this.match.score.threeFirstTeam, Validators.required],
        threeSecondTeam: [this.match.score.threeSecondTeam, Validators.required],
        fourFirstTeam: [this.match.score.fourFirstTeam, Validators.required],
        fourSecondTeam: [this.match.score.fourSecondTeam, Validators.required],
        fiveFirstTeam: [this.match.score.fiveFirstTeam, Validators.required],
        fiveSecondTeam: [this.match.score.fiveSecondTeam, Validators.required]
      });
    }
  }

  setLocation() {
    if (this.locationForm.valid) {
      this.location = Object.assign({}, this.locationForm.value);
      this.matchService.setLocationAndTime(this.match.id, this.location).subscribe(() => {
        this.alertify.success('Location set successfully');
      } , error => {
          this.alertify.error(error);
      });
   }
  }

  setScore() {
    if (this.scoreForm.valid) {
      this.score = Object.assign({}, this.scoreForm.value);
      this.matchService.setScore(this.match.id, this.score).subscribe(() => {
        this.alertify.success('Score set successfully');
      } , error => {
          this.alertify.error(error);
      }, () => {
          this.router.navigate(['/matches']);
      });
   }
  }

  cancelInvite() {
    this.inviteService.declineRefereeInvite(this.user.id, this.match.id).subscribe(() => {
      this.alertify.success('Referee invitation canceled');
      this.reloadMatchData();
    } , error => {
        this.alertify.error(error);
    });
  }

  reloadMatchData() {
    this.matchService.getMatch(this.match.id)
    .subscribe((res: Match) => {
      this.match = res;
    }, error => {
      this.alertify.error(error);
    });
  }

}
