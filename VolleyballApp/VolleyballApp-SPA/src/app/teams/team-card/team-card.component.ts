import { Component, Input, OnInit } from '@angular/core';
import { ShortenPipe } from 'ngx-pipes';
import { Team } from 'src/app/_models/team';

@Component({
  selector: 'app-team-card',
  templateUrl: './team-card.component.html',
  styleUrls: ['./team-card.component.scss'],
  providers: [ShortenPipe]
})
export class TeamCardComponent implements OnInit {
  @Input() team: Team;
  constructor() { }

  ngOnInit() {
  }

}
