import { Component, Input, OnInit } from '@angular/core';
import { Team } from 'src/app/_models/team';

@Component({
  selector: 'app-team-card',
  templateUrl: './team-card.component.html',
  styleUrls: ['./team-card.component.scss']
})
export class TeamCardComponent implements OnInit {
  @Input() team: Team;
  constructor() { }

  ngOnInit() {
  }

}
