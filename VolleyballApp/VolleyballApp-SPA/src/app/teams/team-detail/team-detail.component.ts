import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Team } from 'src/app/_models/team';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.css']
})
export class TeamDetailComponent implements OnInit {
  team: Team;

  constructor(private root: ActivatedRoute) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.team = data.team;
    });
  }

}
