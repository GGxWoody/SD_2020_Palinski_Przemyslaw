import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ShortenPipe } from 'ngx-pipes';
import { Team } from 'src/app/_models/team';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-team-card',
  templateUrl: './team-card.component.html',
  styleUrls: ['./team-card.component.scss'],
  providers: [ShortenPipe]
})
export class TeamCardComponent implements OnInit {
  @Input() team: Team;
  user: User = JSON.parse(localStorage.getItem('user'));
  constructor(private router: Router) { }

  ngOnInit() {
  }
}
